using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode.Components;
using Unity.Netcode;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories
{
    public abstract class PvPFactory : PvPBuilding, IPvPFactory, IDroneConsumerProvider
    {
        private IPvPUnitSpawnPositionFinder _unitSpawnPositionFinder;
        private IPvPUnitSpawnDecider _unitSpawnDecider;
        private Pool<PvPUnit, PvPBuildableActivationArgs> _unitPool;

        public abstract UnitCategory UnitCategory { get; }

        public event EventHandler<PvPUnitStartedEventArgs> UnitStarted;
        public event EventHandler<PvPUnitCompletedEventArgs> UnitCompleted;
        public event EventHandler NewUnitChosen;
        public event EventHandler UnitUnderConstructionDestroyed;
        // public event EventHandler<PvPUnitStartedEventArgs> NewFactoryChosen;

        #region Properties
        public abstract LayerMask UnitLayerMask { get; }
        public IPvPUnit UnitUnderConstruction { get; set; }
        public override bool IsBoostable => true;

        private ObservableValue<bool> _isUnitPaused;
        public IObservableValue<bool> IsUnitPaused
        {
            get { return _isUnitPaused; }
        }

        private IPvPBuildableWrapper<IPvPUnit> _unitWrapper;
        public IPvPBuildableWrapper<IPvPUnit> UnitWrapper
        {
            set
            {
                // Logging.Log(Tags.FACTORY, $"{_unitWrapper} > {value}");
                Assert.AreEqual(PvPBuildableState.Completed, BuildableState);
                if (!ReferenceEquals(_unitWrapper, value))
                {
                    if (IsServer && _unitWrapper != null)
                    {
                        CleanUpDroneConsumer();
                        DestroyUnitUnderConstruction();
                        _isUnitPaused.Value = false;
                        OnIsUnitPausedValueChanged(false);
                        _unitPool = null;
                    }

                    _unitWrapper = value;

                    if (IsServer && _unitWrapper != null)
                    {
                        SetupDroneConsumer(_unitWrapper.Buildable.NumOfDronesRequired, showDroneFeedback: false);
                        EnsureDroneConsumerHasHighestPriority();
                        _unitPool = PvPFactoryProvider.PoolProviders.UnitToPoolMap.GetPool(_unitWrapper.Buildable);
                        Assert.IsNotNull(_unitPool);
                        NewUnitChosen?.Invoke(this, EventArgs.Empty);
                        OnNewUnitChosen();
                    }
                }
            }
            get { return _unitWrapper; }
        }

        private int _variantIndex = -1;
        public int VariantIndex
        {
            get { return _variantIndex; }
            set { _variantIndex = value; }
        }

        public int NumOfDrones
        {
            get
            {
                return DroneConsumer != null ? DroneConsumer.NumOfDrones : 0;
            }
        }

        public AudioClip selectedSound;
        public AudioClipWrapper SelectedSound { get; private set; }

        public AudioClip unitSelectedSound;
        public AudioClipWrapper UnitSelectedSound { get; private set; }
        #endregion Properties

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            _isUnitPaused = new ObservableValue<bool>(false);

            Assert.IsNotNull(selectedSound);
            SelectedSound = new AudioClipWrapper(selectedSound);

            Assert.IsNotNull(unitSelectedSound);
            UnitSelectedSound = new AudioClipWrapper(unitSelectedSound);
        }

        public override void Activate(PvPBuildingActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _unitSpawnPositionFinder = CreateSpawnPositionFinder();
            _unitSpawnDecider = new PvPCompositeSpawnDecider(
                    new PvPCooldownSpawnDecider(
                        new PvPUnitSpawnTimer(
                            this,
                            TimeBC.Instance)),
                    new PvPSpaceSpawnDecider(
                        this,
                        _unitSpawnPositionFinder),
                    new PvPPopulationLimitSpawnDecider(
                        ParentCruiser.UnitMonitor,
                        Constants.POPULATION_LIMIT));

            // sava added
            UnitUnderConstruction = null;
        }

        protected abstract IPvPUnitSpawnPositionFinder CreateSpawnPositionFinder();

        protected override void OnSingleClick()
        {
            base.OnSingleClick();

            if (/* Faction == Faction.Blues && */ IsClient && IsOwner)
            {
                _uiManager.ShowFactoryUnits(this);
            }
        }

        // PERF  Don't need to do this every update :)
        protected override void OnUpdate()
        {
            // Logging.Verbose(Tags.FACTORY, $"UnitWrapper: {UnitWrapper}  _isUnitPaused.Value: {_isUnitPaused.Value}  UnitUnderConstruction: {UnitUnderConstruction}");
            if (UnitWrapper != null)
            {
                // Logging.Verbose(Tags.FACTORY, $"Can spawn: {_unitSpawnDecider.CanSpawnUnit(UnitWrapper.Buildable)}");
            }
            if (UnitWrapper != null
                && !_isUnitPaused.Value
                && (UnitUnderConstruction == null || UnitUnderConstruction.BuildableState == PvPBuildableState.Completed)
                && _unitSpawnDecider.CanSpawnUnit(UnitWrapper.Buildable))
            {
                StartBuildingUnit();
            }
        }

        private void StartBuildingUnit()
        {
            // Logging.LogMethod(Tags.FACTORY);

            if (EnemyCruiser == null || ParentCruiser == null)
            {
                return;
            }
            PvPBuildableActivationArgs activationArgs = new PvPBuildableActivationArgs(ParentCruiser, EnemyCruiser, _cruiserSpecificFactories, VariantIndex);
            UnitUnderConstruction = _unitPool.GetItem(activationArgs);

            if (UnitUnderConstruction == null)
            {
                Logging.Warn(Tags.FACTORY, $"Failed to get unit from pool - reached creation limit");
                return;
            }

            Assert.IsNotNull(UnitUnderConstruction);
            UnitUnderConstruction.DroneConsumerProvider = this;

            Vector3 spawnPosition = _unitSpawnPositionFinder.FindSpawnPosition(UnitUnderConstruction);
            UnitUnderConstruction.Position = spawnPosition;
            UnitUnderConstruction.Rotation = transform.rotation;

            if (UnitUnderConstruction.GameObject.GetComponent<NetworkTransform>() != null)
            {
                UnitUnderConstruction.GameObject.GetComponent<NetworkTransform>().Teleport(spawnPosition, transform.rotation, UnitUnderConstruction.GameObject.transform.localScale);
            }

            UnitUnderConstruction.StartedConstruction += Unit_BuildingStarted;
            UnitUnderConstruction.CompletedBuildable += Unit_CompletedBuildable;
            UnitUnderConstruction.Destroyed += UnitUnderConstruction_Destroyed;

            UnitUnderConstruction.AddBuildRateBoostProviders(_parentSlot.BoostProviders);
            UnitUnderConstruction.StartConstruction();

            /*            if (UnitUnderConstruction.ParentCruiser.IsPlayerCruiser)
                        {
                            string logName = UnitUnderConstruction.PrefabName.ToUpper().Replace("(CLONE)", "");
            #if LOG_ANALYTICS
                Debug.Log("Analytics: " + logName);
            #endif
                            ApplicationModel applicationModel = ApplicationModel;
                            try
                            {
                                AnalyticsService.Instance.CustomData("Battle_Buildable_Unit", DataProvider.GameModel.Analytics(ApplicationModel.Mode.ToString(), logName, ApplicationModel.UserWonSkirmish));
                                AnalyticsService.Instance.Flush();
                            }
                            catch (ConsentCheckException ex)
                            {
                                Debug.Log(ex.Message);
                            }
                        }*/

        }

        protected virtual void Unit_BuildingStarted(object sender, EventArgs e)
        {
            // Logging.Log(Tags.FACTORY, sender.ToString());

            IPvPUnit unit = sender.Parse<IPvPUnit>();


            UnitStarted?.Invoke(this, new PvPUnitStartedEventArgs(unit));
            if (IsServer)
            {
                PvPBuildable<PvPBuildableActivationArgs> buildable_building = sender.Parse<PvPBuildable<PvPBuildableActivationArgs>>();
                if (buildable_building != null && buildable_building._parent.GetComponent<NetworkObject>() != null)
                {
                    OnUnit_BuildingStarted(buildable_building._parent.GetComponent<NetworkObject>().NetworkObjectId);
                }
            }
        }

        protected virtual void OnUnit_BuildingStarted(ulong objectId)
        {
            if (!IsHost)
            {

                NetworkObject obj = PvPBattleSceneGodClient.Instance.GetNetworkObject(objectId);
                IPvPUnit unit = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPUnit>>().Buildable.Parse<IPvPUnit>();
                UnitUnderConstruction = unit;
                UnitStarted?.Invoke(this, new PvPUnitStartedEventArgs(unit));
            }
        }

        protected virtual void OnNewUnitChosen()
        {
            if (IsClient)
                NewUnitChosen?.Invoke(this, EventArgs.Empty);
        }

        private void Unit_CompletedBuildable(object sender, EventArgs e)
        {
            // Logging.Log(Tags.FACTORY, sender.ToString());

            UnitCompleted?.Invoke(this, new PvPUnitCompletedEventArgs(UnitUnderConstruction));
            CleanUpUnitUnderConstruction();
            if (IsServer)
            {
                PvPBuildable<PvPBuildableActivationArgs> buildable_building = sender.Parse<PvPBuildable<PvPBuildableActivationArgs>>();
                if (buildable_building != null && buildable_building._parent.GetComponent<NetworkObject>() != null)
                {
                    OnUnit_CompletedBuildable(buildable_building._parent.GetComponent<NetworkObject>().NetworkObjectId);
                }
                // else
                // {
                //     PvPUnit buildable_unit = sender.Parse<PvPUnit>();
                //     if (buildable_unit != null && buildable_unit._parent.GetComponent<NetworkObject>() != null)
                //     {
                //         OnUnit_CompletedBuildable(buildable_unit._parent.GetComponent<NetworkObject>().NetworkObjectId);
                //     }
                // }
            }
        }

        protected virtual void OnUnit_CompletedBuildable(ulong objectId)
        {
            if (IsClient)
            {
                // NetworkObject[] objs = FindObjectsByType<NetworkObject>(FindObjectsSortMode.None);
                // foreach (NetworkObject obj in objs)
                // {
                //     if (obj.NetworkObjectId == objectId)
                //     {
                NetworkObject obj = PvPBattleSceneGodClient.Instance.GetNetworkObject(objectId);
                IPvPUnit unit = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPUnit>>().Buildable.Parse<IPvPUnit>();
                UnitCompleted?.Invoke(this, new PvPUnitCompletedEventArgs(unit));
                //   CleanUpUnitUnderConstruction();
                UnitUnderConstruction = null;
                //     }
                // }
            }
        }

        private void UnitUnderConstruction_Destroyed(object sender, DestroyedEventArgs e)
        {
            // Logging.Log(Tags.FACTORY, sender.ToString());

            CleanUpUnitUnderConstruction();
            UnitUnderConstructionDestroyed?.Invoke(this, EventArgs.Empty);
            OnUnitUnderConstruction_Destroyed();
        }

        protected virtual void OnUnitUnderConstruction_Destroyed()
        {
            if (IsClient)
            {
                UnitUnderConstructionDestroyed?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The unit under construction will request a drone consumer.  The factory
        /// keeps hold of a drone consumer for the current unit, so simply pass
        /// on the factory's drone consumer.
        /// </summary>
        public IDroneConsumer RequestDroneConsumer(int numOfDronesRequired)
        {
            // Logging.LogMethod(Tags.FACTORY);

            Assert.IsNotNull(DroneConsumer);
            Assert.AreEqual(DroneConsumer.NumOfDronesRequired, numOfDronesRequired, "DroneConsumer.NumOfDronesRequired: " + DroneConsumer.NumOfDronesRequired + " != " + numOfDronesRequired);
            return DroneConsumer;
        }

        public void ActivateDroneConsumer(IDroneConsumer droneConsumer) { }

        public void ReleaseDroneConsumer(IDroneConsumer droneConsumer) { }

        protected override void OnDestroyed()
        {
            // Logging.LogMethod(Tags.FACTORY);

            base.OnDestroyed();
            DestroyUnitUnderConstruction();
        }

        private void DestroyUnitUnderConstruction()
        {
            // Logging.LogMethod(Tags.FACTORY);

            if (UnitUnderConstruction != null
                && !UnitUnderConstruction.IsDestroyed
                && UnitUnderConstruction.BuildableState != PvPBuildableState.Completed)
            {
                UnitUnderConstruction.Destroy();
            }
        }

        private void CleanUpUnitUnderConstruction()
        {
            // Logging.LogMethod(Tags.FACTORY);

            UnitUnderConstruction.StartedConstruction -= Unit_BuildingStarted;
            UnitUnderConstruction.CompletedBuildable -= Unit_CompletedBuildable;
            UnitUnderConstruction.Destroyed -= UnitUnderConstruction_Destroyed;
            UnitUnderConstruction = null;
        }

        public void StartBuildingUnit(IPvPBuildableWrapper<IPvPUnit> unit, int variantIndex)
        {
            // Logging.Log(Tags.FACTORY, unit?.ToString());
            UnitWrapper = unit;
            VariantIndex = variantIndex;
            if (IsClient)
            {
                OnStartBuildingUnit(UnitWrapper.Buildable.Category, UnitWrapper.Buildable.PrefabName, VariantIndex);
            }
        }

        public void StartBuildingUnit(IPvPBuildableWrapper<IPvPUnit> unit)
        {
            // Logging.Log(Tags.FACTORY, unit?.ToString());
            UnitWrapper = unit;
            if (IsClient)
            {
                OnStartBuildingUnit(UnitWrapper.Buildable.Category, UnitWrapper.Buildable.PrefabName, VariantIndex);
            }
        }


        public void StopBuildingUnit()
        {
            // Logging.LogMethod(Tags.FACTORY);
            UnitWrapper = null;
        }

        public void PauseBuildingUnit()
        {
            if (IsServer)
            {
                if (UnitWrapper != null
                    && !_isUnitPaused.Value)
                {
                    _droneConsumerProvider.ReleaseDroneConsumer(DroneConsumer);
                    _isUnitPaused.Value = true;
                    OnIsUnitPausedValueChanged(true);
                }
            }
            else
                OnPauseBuildingUnit();
        }

        protected virtual void OnPauseBuildingUnit()
        {
            if (IsServer)
                PauseBuildingUnit();
        }

        protected virtual void OnIsUnitPausedValueChanged(bool isUnitPaused)
        {
            if (!IsHost)
                _isUnitPaused.Value = isUnitPaused;
        }

        public void ResumeBuildingUnit()
        {
            // Logging.LogMethod(Tags.FACTORY);



            if (IsServer)
            {
                if (_isUnitPaused.Value)
                {
                    Assert.IsNotNull(UnitWrapper);
                    _droneConsumerProvider.ActivateDroneConsumer(DroneConsumer);
                    EnsureDroneConsumerHasHighestPriority();
                    _isUnitPaused.Value = false;
                    OnIsUnitPausedValueChanged(false);
                }
            }
            else
                OnResumeBuildingUnit();
        }

        protected virtual void OnResumeBuildingUnit()
        {
            if (IsServer)
            {
                ResumeBuildingUnit();
            }
        }

        private void EnsureDroneConsumerHasHighestPriority()
        {
            // Logging.LogMethod(Tags.FACTORY);

            if (DroneConsumer.State == DroneConsumerState.Idle)
            {
                ParentCruiser.DroneFocuser.ToggleDroneConsumerFocus(DroneConsumer, isTriggeredByPlayer: false);
            }
        }

        protected override void ToggleDroneConsumerFocusCommandExecute()
        {
            // Logging.Log(Tags.FACTORY, $"_isUnitPaused.Value: {_isUnitPaused.Value}");

            if (_isUnitPaused.Value && IsServer)
            {
                // Cannot focus on drone consumer if they are not activated.
                // Hence, activate drone consumer before focusing on them :)
                _droneConsumerProvider.ActivateDroneConsumer(DroneConsumer);
                _isUnitPaused.Value = false;
                OnIsUnitPausedValueChanged(false);
            }

            base.ToggleDroneConsumerFocusCommandExecute();
        }


        protected virtual void OnStartBuildingUnit(UnitCategory category, string prefabName, int variantIndex)
        {
        }

    }
}

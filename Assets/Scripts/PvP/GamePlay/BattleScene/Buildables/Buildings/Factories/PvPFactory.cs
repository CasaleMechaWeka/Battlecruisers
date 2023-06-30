using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode.Components;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories
{
    public abstract class PvPFactory : PvPBuilding, IPvPFactory, IPvPDroneConsumerProvider
    {
        private IPvPUnitSpawnPositionFinder _unitSpawnPositionFinder;
        private IPvPUnitSpawnDecider _unitSpawnDecider;
        private IPvPPool<PvPUnit, PvPBuildableActivationArgs> _unitPool;

        public abstract PvPUnitCategory UnitCategory { get; }

        public event EventHandler<PvPUnitStartedEventArgs> UnitStarted;
        public event EventHandler<PvPUnitCompletedEventArgs> UnitCompleted;
        public event EventHandler NewUnitChosen;
        public event EventHandler UnitUnderConstructionDestroyed;

        #region Properties
        public abstract LayerMask UnitLayerMask { get; }
        public IPvPUnit UnitUnderConstruction { get; private set; }
        public override bool IsBoostable => true;

        private PvPObservableValue<bool> _isUnitPaused;
        public IPvPObservableValue<bool> IsUnitPaused
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
                        _unitPool = null;
                    }

                    _unitWrapper = value;

                    if (IsServer && _unitWrapper != null)
                    {
                        SetupDroneConsumer(_unitWrapper.Buildable.NumOfDronesRequired, showDroneFeedback: false);
                        EnsureDroneConsumerHasHighestPriority();
                        _unitPool = _factoryProvider.PoolProviders.UnitToPoolMap.GetPool(_unitWrapper.Buildable);
                        Assert.IsNotNull(_unitPool);
                        NewUnitChosen?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            get { return _unitWrapper; }
        }

        public int NumOfDrones
        {
            get
            {
                return DroneConsumer != null ? DroneConsumer.NumOfDrones : 0;
            }
        }

        public AudioClip selectedSound;
        public IPvPAudioClipWrapper SelectedSound { get; private set; }

        public AudioClip unitSelectedSound;
        public IPvPAudioClipWrapper UnitSelectedSound { get; private set; }
        #endregion Properties

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _isUnitPaused = new PvPObservableValue<bool>(false);

            Assert.IsNotNull(selectedSound);
            SelectedSound = new PvPAudioClipWrapper(selectedSound);

            Assert.IsNotNull(unitSelectedSound);
            UnitSelectedSound = new PvPAudioClipWrapper(unitSelectedSound);
        }

        public override void Activate(PvPBuildingActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _unitSpawnPositionFinder = CreateSpawnPositionFinder();
            _unitSpawnDecider = _factoryProvider.SpawnDeciderFactory.CreateSpawnDecider(this, _unitSpawnPositionFinder);
        }

        protected abstract IPvPUnitSpawnPositionFinder CreateSpawnPositionFinder();

        protected override void OnSingleClick()
        {
            base.OnSingleClick();

            if (/* Faction == PvPFaction.Blues && */ IsClient && IsOwner)
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

        private async void StartBuildingUnit()
        {
            // Logging.LogMethod(Tags.FACTORY);

            if (EnemyCruiser == null || ParentCruiser == null)
            {
                return;
            }
            PvPBuildableActivationArgs activationArgs = new PvPBuildableActivationArgs(ParentCruiser, EnemyCruiser, _cruiserSpecificFactories);
            UnitUnderConstruction = await _unitPool.GetItem(activationArgs);
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
                            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
                            try
                            {
                                AnalyticsService.Instance.CustomData("Battle_Buildable_Unit", applicationModel.DataProvider.GameModel.Analytics(applicationModel.Mode.ToString(), logName, applicationModel.UserWonSkirmish));
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
        }

        private void Unit_CompletedBuildable(object sender, EventArgs e)
        {
            // Logging.Log(Tags.FACTORY, sender.ToString());

            UnitCompleted?.Invoke(this, new PvPUnitCompletedEventArgs(UnitUnderConstruction));
            CleanUpUnitUnderConstruction();
        }

        private void UnitUnderConstruction_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            // Logging.Log(Tags.FACTORY, sender.ToString());

            CleanUpUnitUnderConstruction();
            UnitUnderConstructionDestroyed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The unit under construction will request a drone consumer.  The factory
        /// keeps hold of a drone consumer for the current unit, so simply pass
        /// on the factory's drone consumer.
        /// </summary>
        public IPvPDroneConsumer RequestDroneConsumer(int numOfDronesRequired)
        {
            // Logging.LogMethod(Tags.FACTORY);

            Assert.IsNotNull(DroneConsumer);
            Assert.AreEqual(DroneConsumer.NumOfDronesRequired, numOfDronesRequired, "DroneConsumer.NumOfDronesRequired: " + DroneConsumer.NumOfDronesRequired + " != " + numOfDronesRequired);
            return DroneConsumer;
        }

        public void ActivateDroneConsumer(IPvPDroneConsumer droneConsumer) { }

        public void ReleaseDroneConsumer(IPvPDroneConsumer droneConsumer) { }

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

        public void StartBuildingUnit(IPvPBuildableWrapper<IPvPUnit> unit)
        {
            // Logging.Log(Tags.FACTORY, unit?.ToString());
            UnitWrapper = unit;
            OnStartBuildingUnit(UnitWrapper.Buildable.Category, UnitWrapper.Buildable.PrefabName);
        }

        public void StopBuildingUnit()
        {
            // Logging.LogMethod(Tags.FACTORY);
            UnitWrapper = null;
        }

        public void PauseBuildingUnit()
        {
            // Logging.LogMethod(Tags.FACTORY);

            if (UnitWrapper != null
                && !_isUnitPaused.Value)
            {
                _droneConsumerProvider.ReleaseDroneConsumer(DroneConsumer);
                _isUnitPaused.Value = true;
            }
        }

        public void ResumeBuildingUnit()
        {
            // Logging.LogMethod(Tags.FACTORY);

            if (_isUnitPaused.Value)
            {
                Assert.IsNotNull(UnitWrapper);

                _droneConsumerProvider.ActivateDroneConsumer(DroneConsumer);
                EnsureDroneConsumerHasHighestPriority();
                _isUnitPaused.Value = false;
            }
        }

        private void EnsureDroneConsumerHasHighestPriority()
        {
            // Logging.LogMethod(Tags.FACTORY);

            if (DroneConsumer.State == PvPDroneConsumerState.Idle)
            {
                ParentCruiser.DroneFocuser.ToggleDroneConsumerFocus(DroneConsumer, isTriggeredByPlayer: false);
            }
        }

        protected override void ToggleDroneConsumerFocusCommandExecute()
        {
            // Logging.Log(Tags.FACTORY, $"_isUnitPaused.Value: {_isUnitPaused.Value}");

            if (_isUnitPaused.Value)
            {
                // Cannot focus on drone consumer if they are not activated.
                // Hence, activate drone consumer before focusing on them :)
                _droneConsumerProvider.ActivateDroneConsumer(DroneConsumer);
                _isUnitPaused.Value = false;
            }

            base.ToggleDroneConsumerFocusCommandExecute();
        }


        protected virtual void OnStartBuildingUnit(PvPUnitCategory category, string prefabName)
        {
        }

    }
}

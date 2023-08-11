using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using Unity.Netcode.Components;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Offensive
{
    public class PvPNukeLauncherController : PvPBuilding
    {
        private PvPNukeSpinner _spinner;
        private IPvPNukeStats _nukeStats;
        private PvPNukeController _launchedNuke;

        public PvPSiloHalfController leftSiloHalf, rightSiloHalf;
        public PvPNukeController nukeMissilePrefab;

        private IPvPAudioClipWrapper _nukeImpactSound;
        public AudioClip nukeImpactSound;

        private const float SILO_HALVES_ROTATE_SPEED_IN_M_PER_S = 15;
        private const float SILO_TARGET_ANGLE_IN_DEGREES = 45;
        private static Vector3 NUKE_SPAWN_POSITION_ADJUSTMENT = new Vector3(0, -0.3f, 0);

        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.PvPCompleted.Ultra;
        public override PvPTargetValue TargetValue => PvPTargetValue.High;

        protected override void AddBuildRateBoostProviders(
            IPvPGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IPvPBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            PvPHelper.AssertIsNotNull(leftSiloHalf, rightSiloHalf, nukeMissilePrefab);

            leftSiloHalf.StaticInitialise();
            rightSiloHalf.StaticInitialise();

            _spinner = gameObject.GetComponentInChildren<PvPNukeSpinner>();
            Assert.IsNotNull(_spinner);
            _spinner.StaticInitialise();

            _nukeStats = GetComponent<PvPNukeProjectileStats>();
            Assert.IsNotNull(_nukeStats);
            AddAttackCapability(PvPTargetType.Cruiser);
            AddDamageStats(new PvPDamageCapability(_nukeStats.Damage, AttackCapabilities));

            Assert.IsNotNull(nukeImpactSound);
            _nukeImpactSound = new PvPAudioClipWrapper(nukeImpactSound);
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(factoryProvider);

            leftSiloHalf.Initialise(_movementControllerFactory, SILO_HALVES_ROTATE_SPEED_IN_M_PER_S, SILO_TARGET_ANGLE_IN_DEGREES);
            rightSiloHalf.Initialise(_movementControllerFactory, SILO_HALVES_ROTATE_SPEED_IN_M_PER_S, SILO_TARGET_ANGLE_IN_DEGREES);

            _spinner.Initialise(_movementControllerFactory);
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider, IPvPUIManager uiManager)
        {
            base.Initialise(factoryProvider, uiManager);
            leftSiloHalf.Initialise(_movementControllerFactory, SILO_HALVES_ROTATE_SPEED_IN_M_PER_S, SILO_TARGET_ANGLE_IN_DEGREES);
            rightSiloHalf.Initialise(_movementControllerFactory, SILO_HALVES_ROTATE_SPEED_IN_M_PER_S, SILO_TARGET_ANGLE_IN_DEGREES);

            _spinner.Initialise(_movementControllerFactory);
        }


        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();

                _spinner.StartRotating();
                _spinner.StopRotating();
                _spinner.Renderer.enabled = false;

                CreateNuke();

                leftSiloHalf.ReachedDesiredAngle += SiloHalf_ReachedDesiredAngle;

                leftSiloHalf.StartRotating();
                rightSiloHalf.StartRotating();
                OnBuildableCompletedClientRpc();
            }
            if (IsClient)
            {
                OnBuildableCompleted_PvPClient();

                _spinner.StartRotating();
                _spinner.StopRotating();
                _spinner.Renderer.enabled = false;

           //     leftSiloHalf.ReachedDesiredAngle += SiloHalf_ReachedDesiredAngle;

                leftSiloHalf.StartRotating();
                rightSiloHalf.StartRotating();
            }
          

        }

        private async void CreateNuke()
        {
            PvPProjectileKey prefabKey  = new PvPProjectileKey("PvPNuke");
            var isLoaded = await SynchedServerData.Instance.TrySpawnCruiserDynamicSynchronously(prefabKey, nukeMissilePrefab);
            if(isLoaded)
            {
                _launchedNuke = Instantiate(nukeMissilePrefab);
                _launchedNuke.gameObject.GetComponent<NetworkObject>().Spawn();
                IPvPTargetFilter targetFilter = _factoryProvider.Targets.FilterFactory.CreateExactMatchTargetFilter(EnemyCruiser);
                _launchedNuke.Initialise(_commonStrings, _factoryProvider);
                _launchedNuke.Activate(
                    new PvPTargetProviderActivationArgs<IPvPNukeStats>(
                        transform.position + NUKE_SPAWN_POSITION_ADJUSTMENT,
                        _nukeStats,
                        Vector2.zero,
                        targetFilter,
                        this,
                        _nukeImpactSound,
                        EnemyCruiser));

                // Make nuke face upwards (rotation is set in Initialise() above)
                _launchedNuke.transform.eulerAngles = new Vector3(0, 0, 90);
            }
        }

        private void SiloHalf_ReachedDesiredAngle(object sender, EventArgs e)
        {
            leftSiloHalf.ReachedDesiredAngle -= SiloHalf_ReachedDesiredAngle;
            _launchedNuke.Launch();
        }

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            return new List<SpriteRenderer>()
            {
                transform.FindNamedComponent<SpriteRenderer>("Base"),
                _spinner.Renderer,
                leftSiloHalf.Renderer,
                rightSiloHalf.Renderer
            };
        }


        // sava added
        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();

        // Visibility 
        protected override void OnValueChangedIsEnableRenderes(bool isEnabled)
        {
            if (IsClient)
                base.OnValueChangedIsEnableRenderes(isEnabled);
            if (IsServer)
                OnValueChangedIsEnabledRendersClientRpc(isEnabled);
        }


        // Headbar offset
        protected override void CallRpc_SetHealthbarOffset(Vector2 offset)
        {
            OnSetHealthbarOffsetClientRpc(offset);
        }


        // set Position of PvPBuildable
        protected override void CallRpc_SetPosition(Vector3 pos)
        {
            OnSetPositionClientRpc(pos);
        }

        // Set Rotation of PvPBuildable
        protected override void CallRpc_SetRotation(Quaternion rotation)
        {
            OnSetRotationClientRpc(rotation);
        }

        // Drone Focusing
        protected override void ShareIsDroneConsumerFocusableValueWithClient(bool isFocusable)
        {
            OnShareIsDroneConsumerFocusableValueWithClientRpc(isFocusable);
        }

        // Toggle Drone
        protected override void CallRpc_ToggleDroneConsumerFocusCommandExecute()
        {
            base.CallRpc_ToggleDroneConsumerFocusCommandExecute();
            if (IsClient)
                OnToggleDroneConsumerFocusCommandExecuteServerRpc();
        }


        // Placement Sound
        protected override void PlayPlacementSound()
        {
            base.PlayPlacementSound();

            if (IsServer)
                PlayPlacementSoundClientRpc();
        }

        // Destroy me
        protected override void DestroyMe()
        {
            if (IsServer)
                base.DestroyMe();
            if (IsClient)
                OnDestroyMeServerRpc();
        }

        // Death Sound
        protected override void CallRpc_PlayDeathSound()
        {
            if (IsClient)
                base.CallRpc_PlayDeathSound();
            if (IsServer)
                OnPlayDeathSoundClientRpc();
        }

        // BuildableConstructionCompletedSound
        protected override void PlayBuildableConstructionCompletedSound()
        {
            if (IsClient)
                base.PlayBuildableConstructionCompletedSound();
            if (IsServer)
                PlayBuildableConstructionCompletedSoundClientRpc();
        }
        // ProgressController Visible
        protected override void CallRpc_ProgressControllerVisible(bool isEnabled)
        {
            OnProgressControllerVisibleClientRpc(isEnabled);
        }

        // BuildableStatus
        protected override void OnBuildableStateValueChanged(PvPBuildableState state)
        {
            OnBuildableStateValueChangedClientRpc(state);
        }

        // ClickedRepairButton
        protected override void CallRpc_ClickedRepairButton()
        {
            PvP_RepairableButtonClickedServerRpc();
        }

        // SyncFaction
        protected override void CallRpc_SyncFaction(PvPFaction faction)
        {
            OnSyncFationClientRpc(faction);
        }

        protected override void OnDestroyedEvent()
        {
            if (IsClient)
                base.OnDestroyedEvent();
            if (IsServer)
                OnDestroyedEventClientRpc();
        }


        private void LateUpdate()
        {
            if (IsServer)
            {
                if (PvP_BuildProgress.Value != BuildProgress)
                    PvP_BuildProgress.Value = BuildProgress;
            }
            if (IsClient)
            {
                BuildProgress = PvP_BuildProgress.Value;
            }
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                pvp_Health.Value = maxHealth;
        }

        // ----------------------------------------

        [ClientRpc]
        private void OnValueChangedIsEnabledRendersClientRpc(bool isEnabled)
        {
            OnValueChangedIsEnableRenderes(isEnabled);
        }

        [ClientRpc]
        private void OnSetHealthbarOffsetClientRpc(Vector2 offset)
        {
            HealthBar.Offset = offset;
        }

        [ClientRpc]
        private void OnSetPositionClientRpc(Vector3 pos)
        {
            Position = pos;
        }

        [ClientRpc]
        private void OnSetRotationClientRpc(Quaternion rotation)
        {
            Rotation = rotation;
        }

        [ClientRpc]
        private void OnShareIsDroneConsumerFocusableValueWithClientRpc(bool isFocusable)
        {
            IsDroneConsumerFocusable_PvPClient = isFocusable;
        }

        [ServerRpc]
        private void OnToggleDroneConsumerFocusCommandExecuteServerRpc()
        {
            CallRpc_ToggleDroneConsumerFocusCommandExecute();
        }
        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            OnBuildableCompleted();
        }

        [ClientRpc]
        private void PlayPlacementSoundClientRpc()
        {
            PlayPlacementSound();
        }

        [ServerRpc(RequireOwnership = true)]
        private void OnDestroyMeServerRpc()
        {
            DestroyMe();
        }
        [ClientRpc]
        private void OnPlayDeathSoundClientRpc()
        {
            CallRpc_PlayDeathSound();
        }
        [ClientRpc]
        private void PlayBuildableConstructionCompletedSoundClientRpc()
        {
            PlayBuildableConstructionCompletedSound();
        }

        [ClientRpc]
        private void OnProgressControllerVisibleClientRpc(bool isEnabled)
        {
            _buildableProgress.gameObject.SetActive(isEnabled);
        }

        [ClientRpc]
        protected void OnBuildableStateValueChangedClientRpc(PvPBuildableState state)
        {
            BuildableState = state;
        }

        [ServerRpc(RequireOwnership = true)]
        private void PvP_RepairableButtonClickedServerRpc()
        {
            IPvPDroneConsumer repairDroneConsumer = ParentCruiser.RepairManager.GetDroneConsumer(this);
            ParentCruiser.DroneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);
        }

        [ClientRpc]
        private void OnSyncFationClientRpc(PvPFaction faction)
        {
            Faction = faction;
        }

        [ServerRpc(RequireOwnership = true)]
        private void OnStartBuildingUnitServerRpc(PvPUnitCategory category, string prefabName)
        {
            PvPUnitKey _unitKey = new PvPUnitKey(category, prefabName);
            //    UnitWrapper = PvPBattleSceneGodServer.Instance.prefabFactory.GetUnitWrapperPrefab(_unitKey);
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            OnDestroyedEvent();
        }
    }
}

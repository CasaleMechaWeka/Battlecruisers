using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Targets.TargetFinders.Filters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.UI.Sound;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Offensive
{
    public class PvPNukeLauncherController : PvPBuilding
    {
        private PvPNukeSpinner _spinner;
        private ProjectileStats _nukeStats;
        private PvPNukeController _launchedNuke;

        public PvPSiloHalfController leftSiloHalf, rightSiloHalf;
        public PvPNukeController nukeMissilePrefab;

        private AudioClipWrapper _nukeImpactSound;
        public AudioClip nukeImpactSound;

        private const float SILO_HALVES_ROTATE_SPEED_IN_M_PER_S = 15;
        private const float SILO_TARGET_ANGLE_IN_DEGREES = 45;
        private static Vector3 NUKE_SPAWN_POSITION_ADJUSTMENT = new Vector3(0, -0.3f, 0);

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;
        public override TargetValue TargetValue => TargetValue.High;

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            PvPHelper.AssertIsNotNull(leftSiloHalf, rightSiloHalf, nukeMissilePrefab);

            leftSiloHalf.StaticInitialise();
            rightSiloHalf.StaticInitialise();

            _spinner = gameObject.GetComponentInChildren<PvPNukeSpinner>();
            Assert.IsNotNull(_spinner);
            _spinner.StaticInitialise();

            _nukeStats = GetComponent<ProjectileStats>();
            Assert.IsNotNull(_nukeStats);
            AddAttackCapability(TargetType.Cruiser);
            AddDamageStats(new PvPDamageCapability(_nukeStats.Damage, AttackCapabilities));

            Assert.IsNotNull(nukeImpactSound);
            _nukeImpactSound = new AudioClipWrapper(nukeImpactSound);
        }

        public override void Initialise()
        {
            base.Initialise();
            leftSiloHalf.Initialise(SILO_HALVES_ROTATE_SPEED_IN_M_PER_S, SILO_TARGET_ANGLE_IN_DEGREES);
            rightSiloHalf.Initialise(SILO_HALVES_ROTATE_SPEED_IN_M_PER_S, SILO_TARGET_ANGLE_IN_DEGREES);

            _spinner.Initialise();
        }

        public override void Initialise(PvPUIManager uiManager)
        {
            base.Initialise(uiManager);
            leftSiloHalf.Initialise(SILO_HALVES_ROTATE_SPEED_IN_M_PER_S, SILO_TARGET_ANGLE_IN_DEGREES);
            rightSiloHalf.Initialise(SILO_HALVES_ROTATE_SPEED_IN_M_PER_S, SILO_TARGET_ANGLE_IN_DEGREES);

            _spinner.Initialise();
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
            else
            {
                OnBuildableCompleted_PvPClient();
                _spinner.StartRotating();
                _spinner.StopRotating();
                _spinner.Renderer.enabled = false;
                leftSiloHalf.StartRotating();
                rightSiloHalf.StartRotating();
            }
        }

        private void CreateNuke()
        {
            PvPProjectileKey prefabKey = new PvPProjectileKey("PvPNuke");
            /*            var isLoaded = await SynchedServerData.Instance.TrySpawnCruiserDynamicSynchronously(prefabKey, nukeMissilePrefab);
                        if (isLoaded)
                        {*/
            _launchedNuke = Instantiate(nukeMissilePrefab);
            _launchedNuke.gameObject.GetComponent<NetworkObject>().Spawn();
            ITargetFilter targetFilter = new ExactMatchTargetFilter() { Target = EnemyCruiser };
            _launchedNuke.Initialise();
            _launchedNuke.Activate(
                new ProjectileActivationArgs(
                    transform.position + NUKE_SPAWN_POSITION_ADJUSTMENT,
                    _nukeStats,
                    Vector2.zero,
                    targetFilter,
                    this,
                    _nukeImpactSound,
                    EnemyCruiser));

            // Make nuke face upwards (rotation is set in Initialise() above)
            _launchedNuke.transform.eulerAngles = new Vector3(0, 0, 90);
            //    }
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
            else
                OnDestroyMeServerRpc();
        }

        // Death Sound
        protected override void CallRpc_PlayDeathSound()
        {
            if (IsServer)
            {
                OnPlayDeathSoundClientRpc();
                base.CallRpc_PlayDeathSound();
            }
            else
                base.CallRpc_PlayDeathSound();
        }

        // BuildableConstructionCompletedSound
        protected override void PlayBuildableConstructionCompletedSound()
        {
            if (IsServer)
                PlayBuildableConstructionCompletedSoundClientRpc();
            else
                base.PlayBuildableConstructionCompletedSound();
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
        protected override void CallRpc_SyncFaction(Faction faction)
        {
            OnSyncFationClientRpc(faction);
        }

        protected override void OnDestroyedEvent()
        {
            if (IsServer)
            {
                OnDestroyedEventClientRpc();
                base.OnDestroyedEvent();
            }
            else
                base.OnDestroyedEvent();
        }


        private void LateUpdate()
        {
            if (IsServer)
            {
                if (PvP_BuildProgress.Value != BuildProgress)
                    PvP_BuildProgress.Value = BuildProgress;
            }
            else
            {
                BuildProgress = PvP_BuildProgress.Value;
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
                pvp_Health.Value = maxHealth;
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        // ----------------------------------------




        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            if (!IsHost)
                OnBuildableCompleted();
        }

        [ClientRpc]
        private void PlayPlacementSoundClientRpc()
        {
            if (!IsHost)
                base.PlayPlacementSound();
        }

        [ServerRpc(RequireOwnership = true)]
        private void OnDestroyMeServerRpc()
        {
            DestroyMe();
        }
        [ClientRpc]
        private void OnPlayDeathSoundClientRpc()
        {
            if (!IsHost)
                CallRpc_PlayDeathSound();
        }
        [ClientRpc]
        private void PlayBuildableConstructionCompletedSoundClientRpc()
        {
            if (!IsHost)
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
            if (!IsHost)
                BuildableState = state;
        }

        [ServerRpc(RequireOwnership = true)]
        private void PvP_RepairableButtonClickedServerRpc()
        {
            IDroneConsumer repairDroneConsumer = ParentCruiser.RepairManager.GetDroneConsumer(this);
            ParentCruiser.DroneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);
        }

        [ClientRpc]
        private void OnSyncFationClientRpc(Faction faction)
        {
            if (!IsHost)
                Faction = faction;
        }

        [ServerRpc(RequireOwnership = true)]
        private void OnStartBuildingUnitServerRpc(UnitCategory category, string prefabName)
        {
            PvPUnitKey _unitKey = new PvPUnitKey(category, prefabName);
            //    UnitWrapper = PvPBattleSceneGodServer.Instance.prefabFactory.GetUnitWrapperPrefab(_unitKey);
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
                OnDestroyedEvent();
        }
    }
}

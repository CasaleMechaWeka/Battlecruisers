using Unity.Netcode;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.UI.Sound;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public class PvPRocketLauncherController : PvPOffenseTurret
    {
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.RocketLauncher;
        protected override SoundKey FiringSound => SoundKeys.Firing.RocketLauncher;

        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();










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


        // Rpcs









        [ClientRpc]
        private void OnPlayDeathSoundClientRpc()
        {
            if (!IsHost)
                CallRpc_PlayDeathSound();
        }



        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
                OnDestroyedEvent();
        }
        protected override ObservableCollection<IBoostProvider> TurretFireRateBoostProviders
        {
            get
            {
                return _cruiserSpecificFactories.GlobalBoostProviders.RocketBuildingsFireRateBoostProviders;
            }
        }
        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> rocketBuildingsBuildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, rocketBuildingsBuildRateBoostProvidersList);
            rocketBuildingsBuildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.RocketBuildingsProviders);
        }
    }
}
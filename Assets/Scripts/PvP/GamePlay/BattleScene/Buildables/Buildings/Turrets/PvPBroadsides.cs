using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public class PvPBroadsides : PvPOffenseTurret
    {
        protected override SoundKey FiringSound => SoundKeys.Firing.Broadsides;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }


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

        // BuildableConstructionCompletedSound
        protected override void PlayBuildableConstructionCompletedSound()
        {

            if (IsServer)
                PlayBuildableConstructionCompletedSoundClientRpc();
            else
                base.PlayBuildableConstructionCompletedSound();
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
        private void PlayBuildableConstructionCompletedSoundClientRpc()
        {
            if (!IsHost)
                PlayBuildableConstructionCompletedSound();
        }


        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
                OnDestroyedEvent();
        }
    }
}
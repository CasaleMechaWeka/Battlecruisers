using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public class PvPGatlingMortar : PvPOffenseTurret
    {
        // DLC  Have own sound
        protected override SoundKey FiringSound => SoundKeys.Firing.AttackBoat;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.Mortar;


        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();

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
    }
}
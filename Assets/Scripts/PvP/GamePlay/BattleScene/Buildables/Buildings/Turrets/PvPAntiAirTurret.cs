using Unity.Netcode;
using BattleCruisers.UI.Sound;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public class PvPAntiAirTurret : PvPDefenseTurret
    {
        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();
        protected override SoundKey FiringSound => SoundKeys.Firing.AntiAir;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.AntiAirTurret;




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

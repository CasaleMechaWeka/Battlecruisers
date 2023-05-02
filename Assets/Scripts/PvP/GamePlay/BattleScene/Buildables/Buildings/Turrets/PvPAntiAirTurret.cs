using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public class PvPAntiAirTurret : PvPDefenseTurret
    {
        protected override IPvPSoundKey FiringSound => PvPSoundKeys.PvPFiring.AntiAir;
        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.Completed.Buildings.AntiAirTurret;
    }
}

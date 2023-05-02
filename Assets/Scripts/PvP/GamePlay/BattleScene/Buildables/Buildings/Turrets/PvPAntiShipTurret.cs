using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public class PvPAntiShipTurret : PvPDefenseTurret
    {
        protected override IPvPSoundKey FiringSound => PvPSoundKeys.PvPFiring.BigCannon;
        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.Completed.Buildings.AntiShipTurret;
    }
}

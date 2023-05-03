using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public class PvPSamSite : PvPDefenseTurret
    {
        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.Completed.Buildings.SamSite;
        protected override IPvPSoundKey FiringSound => PvPSoundKeys.PvPFiring.Missile;
    }
}
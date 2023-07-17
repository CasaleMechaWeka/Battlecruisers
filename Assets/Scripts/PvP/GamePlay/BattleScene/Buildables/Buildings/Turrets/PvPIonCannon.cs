using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public class PvPIonCannon : PvPOffenseTurret
    {
        // DLC  Have own sound
        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.PvPCompleted.PvPBuildings.Railgun;
    }
}
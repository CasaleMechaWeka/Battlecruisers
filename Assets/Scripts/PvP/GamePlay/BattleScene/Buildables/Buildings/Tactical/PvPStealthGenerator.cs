using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical
{
    public class PvPStealthGenerator : PvPTacticalBuilding, IPvPStealthGenerator
    {
        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.PvPCompleted.PvPBuildings.StealthGenerator;
    }
}

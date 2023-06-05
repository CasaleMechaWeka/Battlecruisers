using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    public class PvPDeleteButtonVisibilityFilter : IPvPFilter<IPvPTarget>
    {
        // Player building
        public bool IsMatch(IPvPTarget target)
        {
            return
                target != null
                && target.Faction == PvPFaction.Blues
                && target.TargetType == PvPTargetType.Buildings
                && target.IsInScene;
        }
    }
}

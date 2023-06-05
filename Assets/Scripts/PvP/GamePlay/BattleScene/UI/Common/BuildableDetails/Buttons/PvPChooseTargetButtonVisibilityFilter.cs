using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    public class PvPChooseTargetButtonVisibilityFilter : IPvPFilter<IPvPTarget>
    {
        // AI buildings or cruiser
        public bool IsMatch(IPvPTarget target)
        {
            return
                target != null
                && target.Faction == PvPFaction.Reds
                && (target.TargetType == PvPTargetType.Buildings
                    || target.TargetType == PvPTargetType.Cruiser);
        }
    }
}

using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters
{
    public class PvPFactionAndTargetTypeFilter : PvPFactionTargetFilter
    {
        private readonly IList<PvPTargetType> _targetTypes;

        public PvPFactionAndTargetTypeFilter(PvPFaction factionToDetect, IList<PvPTargetType> targetTypes)
            : base(factionToDetect)
        {
            _targetTypes = targetTypes;
        }

        public override bool IsMatch(IPvPTarget target)
        {
            bool result
                = base.IsMatch(target)
                    && _targetTypes.Contains(target.TargetType);
            // Logging.Log(Tags.TARGET_FILTER, $"result: {result}  targetType: {target.TargetType}");
            return result;
        }
    }
}

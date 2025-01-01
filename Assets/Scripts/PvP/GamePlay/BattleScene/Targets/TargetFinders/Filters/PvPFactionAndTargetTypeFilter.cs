using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters
{
    public class PvPFactionAndTargetTypeFilter : PvPFactionTargetFilter
    {
        private readonly IList<TargetType> _targetTypes;

        public PvPFactionAndTargetTypeFilter(Faction factionToDetect, IList<TargetType> targetTypes)
            : base(factionToDetect)
        {
            _targetTypes = targetTypes;
        }

        public override bool IsMatch(ITarget target)
        {
            bool result
                = base.IsMatch(target)
                    && _targetTypes.Contains(target.TargetType);
            // Logging.Log(Tags.TARGET_FILTER, $"result: {result}  targetType: {target.TargetType}");
            return result;
        }
    }
}

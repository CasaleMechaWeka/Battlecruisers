using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetFilterFactory : IPvPTargetFilterFactory
    {
        public ITargetFilter CreateTargetFilter(Faction faction)
        {
            return new PvPFactionTargetFilter(faction);
        }

        public ITargetFilter CreateTargetFilter(Faction faction, IList<TargetType> targetTypes)
        {
            return new PvPFactionAndTargetTypeFilter(faction, targetTypes);
        }

        public IPvPExactMatchTargetFilter CreateExactMatchTargetFilter()
        {
            return new PvPExactMatchTargetFilter();
        }

        public IPvPExactMatchTargetFilter CreateExactMatchTargetFilter(ITarget targetToMatch)
        {
            return new PvPExactMatchTargetFilter()
            {
                Target = targetToMatch
            };
        }

        public IPvPExactMatchTargetFilter CreateMulitpleExactMatchTargetFilter()
        {
            return new PvPMultipleExactMatchesTargetFilter();
        }

        public ITargetFilter CreateDummyTargetFilter(bool isMatchResult)
        {
            return new PvPDummyTargetFilter(isMatchResult);
        }

        public ITargetFilter CreateTargetInFrontFilter(IPvPUnit source)
        {
            return new PvPTargetInFrontFilter(source);
        }
    }
}
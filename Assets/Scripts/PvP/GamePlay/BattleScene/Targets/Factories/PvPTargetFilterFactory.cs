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
            return new FactionTargetFilter(faction);
        }

        public ITargetFilter CreateTargetFilter(Faction faction, IList<TargetType> targetTypes)
        {
            return new FactionAndTargetTypeFilter(faction, targetTypes);
        }

        public IExactMatchTargetFilter CreateExactMatchTargetFilter()
        {
            return new ExactMatchTargetFilter();
        }

        public IExactMatchTargetFilter CreateExactMatchTargetFilter(ITarget targetToMatch)
        {
            return new ExactMatchTargetFilter()
            {
                Target = targetToMatch
            };
        }

        public IExactMatchTargetFilter CreateMulitpleExactMatchTargetFilter()
        {
            return new MultipleExactMatchesTargetFilter();
        }

        public ITargetFilter CreateDummyTargetFilter(bool isMatchResult)
        {
            return new DummyTargetFilter(isMatchResult);
        }

        public ITargetFilter CreateTargetInFrontFilter(IPvPUnit source)
        {
            return new PvPTargetInFrontFilter(source);
        }
    }
}
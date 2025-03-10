using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;

namespace BattleCruisers.Targets.Factories
{
    public class TargetFilterFactory : ITargetFilterFactory
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

        public ITargetFilter CreateTargetInFrontFilter(IUnit source)
        {
            return new TargetInFrontFilter(source);
        }
    }
}
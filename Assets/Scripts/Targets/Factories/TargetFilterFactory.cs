using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;

namespace BattleCruisers.Targets.Factories
{
    public static class TargetFilterFactory
    {
        public static ITargetFilter CreateTargetFilter(Faction faction)
        {
            return new FactionTargetFilter(faction);
        }

        public static ITargetFilter CreateTargetFilter(Faction faction, IList<TargetType> targetTypes)
        {
            return new FactionAndTargetTypeFilter(faction, targetTypes);
        }

        public static IExactMatchTargetFilter CreateExactMatchTargetFilter()
        {
            return new ExactMatchTargetFilter();
        }

        public static IExactMatchTargetFilter CreateExactMatchTargetFilter(ITarget targetToMatch)
        {
            return new ExactMatchTargetFilter()
            {
                Target = targetToMatch
            };
        }

        public static IExactMatchTargetFilter CreateMulitpleExactMatchTargetFilter()
        {
            return new MultipleExactMatchesTargetFilter();
        }

        public static ITargetFilter CreateDummyTargetFilter(bool isMatchResult)
        {
            return new DummyTargetFilter(isMatchResult);
        }

        public static ITargetFilter CreateTargetInFrontFilter(IUnit source)
        {
            return new TargetInFrontFilter(source);
        }
    }
}
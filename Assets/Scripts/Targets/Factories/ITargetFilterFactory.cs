using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;

namespace BattleCruisers.Targets.Factories
{
    public interface ITargetFilterFactory
    {
        ITargetFilter CreateTargetFilter(Faction faction);
        ITargetFilter CreateTargetFilter(Faction faction, IList<TargetType> targetTypes);
        ITargetFilter CreateDummyTargetFilter(bool isMatchResult);
        ITargetFilter CreateTargetInFrontFilter(IUnit source);
        IExactMatchTargetFilter CreateExactMatchTargetFilter();
        IExactMatchTargetFilter CreateExactMatchTargetFilter(ITarget targetToMatch);
        IExactMatchTargetFilter CreateMulitpleExactMatchTargetFilter();
    }
}
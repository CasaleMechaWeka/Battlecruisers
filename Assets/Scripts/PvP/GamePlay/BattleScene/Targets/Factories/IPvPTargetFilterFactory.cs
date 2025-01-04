using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetFilterFactory
    {
        ITargetFilter CreateTargetFilter(Faction faction);
        ITargetFilter CreateTargetFilter(Faction faction, IList<TargetType> targetTypes);
        ITargetFilter CreateDummyTargetFilter(bool isMatchResult);
        ITargetFilter CreateTargetInFrontFilter(IPvPUnit source);
        IPvPExactMatchTargetFilter CreateExactMatchTargetFilter();
        IPvPExactMatchTargetFilter CreateExactMatchTargetFilter(ITarget targetToMatch);
        IPvPExactMatchTargetFilter CreateMulitpleExactMatchTargetFilter();
    }
}
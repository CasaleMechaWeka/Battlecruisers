using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetFilterFactory
    {
        IPvPTargetFilter CreateTargetFilter(PvPFaction faction);
        IPvPTargetFilter CreateTargetFilter(PvPFaction faction, IList<PvPTargetType> targetTypes);
        IPvPTargetFilter CreateDummyTargetFilter(bool isMatchResult);
        IPvPTargetFilter CreateTargetInFrontFilter(IPvPUnit source);
        IPvPExactMatchTargetFilter CreateExactMatchTargetFilter();
        IPvPExactMatchTargetFilter CreateExactMatchTargetFilter(IPvPTarget targetToMatch);
        IPvPExactMatchTargetFilter CreateMulitpleExactMatchTargetFilter();
    }
}
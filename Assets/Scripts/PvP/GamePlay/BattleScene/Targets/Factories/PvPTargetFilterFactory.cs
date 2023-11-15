using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetFilterFactory : IPvPTargetFilterFactory
    {
        public IPvPTargetFilter CreateTargetFilter(PvPFaction faction)
        {
            return new PvPFactionTargetFilter(faction);
        }

        public IPvPTargetFilter CreateTargetFilter(PvPFaction faction, IList<PvPTargetType> targetTypes)
        {
            return new PvPFactionAndTargetTypeFilter(faction, targetTypes);
        }

        public IPvPExactMatchTargetFilter CreateExactMatchTargetFilter()
        {
            return new PvPExactMatchTargetFilter();
        }

        public IPvPExactMatchTargetFilter CreateExactMatchTargetFilter(IPvPTarget targetToMatch)
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

        public IPvPTargetFilter CreateDummyTargetFilter(bool isMatchResult)
        {
            return new PvPDummyTargetFilter(isMatchResult);
        }

        public IPvPTargetFilter CreateTargetInFrontFilter(IPvPUnit source)
        {
            return new PvPTargetInFrontFilter(source);
        }
    }
}
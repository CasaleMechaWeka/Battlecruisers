using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters
{
    public class PvPFactionTargetFilter : IPvPTargetFilter
    {
        private readonly PvPFaction _factionToDetect;

        public PvPFactionTargetFilter(PvPFaction faction)
        {
            _factionToDetect = faction;
        }

        public virtual bool IsMatch(IPvPTarget target)
        {
            bool result = target.Faction == _factionToDetect;
            // Logging.Log(Tags.TARGET_FILTER, $"result: {result}  _factionToDetect: {_factionToDetect}");
            return result;
        }
    }
}

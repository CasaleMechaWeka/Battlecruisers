using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters
{
    public class PvPFactionTargetFilter : IPvPTargetFilter
    {
        private readonly Faction _factionToDetect;

        public PvPFactionTargetFilter(Faction faction)
        {
            _factionToDetect = faction;
        }

        public virtual bool IsMatch(IPvPTarget target)
        {
            bool result = target.Faction == _factionToDetect;
            // Logging.Log(Tags.TARGET_FILTER, $"result: {result}  _factionToDetect: {_factionToDetect}");
            return result;
        }

        public virtual bool IsMatch(IPvPTarget target, VariantPrefab variant)
        {
            bool result = target.Faction == _factionToDetect;
            Logging.Log(Tags.TARGET_FILTER, $"result: {result}  _factionToDetect: {_factionToDetect}");
            return result;
        }
    }
}

using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
    public class FactionTargetFilter : ITargetFilter
    {
        private readonly Faction _factionToDetect;

        public FactionTargetFilter(Faction faction)
        {
            _factionToDetect = faction;
        }

        public virtual bool IsMatch(ITarget target)
        {
            bool result = target.Faction == _factionToDetect;
            Logging.Log(Tags.TARGET_FILTER, $"result: {result}  _factionToDetect: {_factionToDetect}");
            return result;
        }
    }
}

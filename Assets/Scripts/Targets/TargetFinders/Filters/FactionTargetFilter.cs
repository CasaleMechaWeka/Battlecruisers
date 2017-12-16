using BattleCruisers.Buildables;

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
            return target.Faction == _factionToDetect;
        }
    }
}

using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
    public class FactionTargetFilter : AliveTargetFilter
    {
        private readonly Faction _factionToDetect;

        public FactionTargetFilter(Faction faction)
        {
            _factionToDetect = faction;
        }

        public override bool IsMatch(ITarget target)
        {
            return 
                base.IsMatch(target)
                && target.Faction == _factionToDetect;
        }
    }
}

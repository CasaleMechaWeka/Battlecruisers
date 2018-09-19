using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
    public class TargetInFrontFilter : AliveTargetFilter
    {
        private readonly IUnit _source;

        public TargetInFrontFilter(IUnit source)
        {
            Assert.IsNotNull(source);
            _source = source;
        }

        public override bool IsMatch(ITarget target)
        {
            return 
                base.IsMatch(target)
                && ((_source.FacingDirection == Direction.Right
                        && target.Position.x > _source.Position.x)
                    || (_source.FacingDirection == Direction.Left
                        && target.Position.x < _source.Position.x));
        }
    }
}

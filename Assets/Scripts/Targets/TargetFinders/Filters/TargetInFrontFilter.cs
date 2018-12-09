using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
    public class TargetInFrontFilter : ITargetFilter
    {
        private readonly IUnit _source;

        public TargetInFrontFilter(IUnit source)
        {
            Assert.IsNotNull(source);
            _source = source;
        }

        public bool IsMatch(ITarget target)
        {
            return 
                (_source.FacingDirection == Direction.Right
                    && target.Position.x > _source.Position.x)
                || (_source.FacingDirection == Direction.Left
                    && target.Position.x < _source.Position.x);
        }
    }
}

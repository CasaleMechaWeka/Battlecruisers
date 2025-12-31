using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters
{
    public class PvPTargetInFrontFilter : ITargetFilter
    {
        private readonly IPvPUnit _source;

        public PvPTargetInFrontFilter(IPvPUnit source)
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

        public bool IsMatch(ITarget target, VariantPrefab variant)
        {
            return
                (_source.FacingDirection == Direction.Right
                    && target.Position.x > _source.Position.x)
                || (_source.FacingDirection == Direction.Left
                    && target.Position.x < _source.Position.x);
        }
    }
}

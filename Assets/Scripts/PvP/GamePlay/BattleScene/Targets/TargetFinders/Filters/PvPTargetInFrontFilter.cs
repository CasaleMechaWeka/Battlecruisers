using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters
{
    public class PvPTargetInFrontFilter : IPvPTargetFilter
    {
        private readonly IPvPUnit _source;

        public PvPTargetInFrontFilter(IPvPUnit source)
        {
            Assert.IsNotNull(source);
            _source = source;
        }

        public bool IsMatch(IPvPTarget target)
        {
            return
                (_source.FacingDirection == PvPDirection.Right
                    && target.Position.x > _source.Position.x)
                || (_source.FacingDirection == PvPDirection.Left
                    && target.Position.x < _source.Position.x);
        }
    }
}

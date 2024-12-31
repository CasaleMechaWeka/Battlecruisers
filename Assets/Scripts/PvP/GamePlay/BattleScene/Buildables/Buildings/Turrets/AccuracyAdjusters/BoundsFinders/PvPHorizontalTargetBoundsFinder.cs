using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders
{
    public class PvPHorizontalTargetBoundsFinder : TargetBoundsFinder
    {
        public PvPHorizontalTargetBoundsFinder(float targetXMarginInM)
            : base(targetXMarginInM, targetYMarginInM: 0)
        {
        }
    }
}

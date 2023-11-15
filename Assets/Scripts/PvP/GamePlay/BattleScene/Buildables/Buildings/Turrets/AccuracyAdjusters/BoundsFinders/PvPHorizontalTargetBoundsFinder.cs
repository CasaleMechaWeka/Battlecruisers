namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders
{
    public class PvPHorizontalTargetBoundsFinder : PvPTargetBoundsFinder
    {
        public PvPHorizontalTargetBoundsFinder(float targetXMarginInM)
            : base(targetXMarginInM, targetYMarginInM: 0)
        {
        }
    }
}

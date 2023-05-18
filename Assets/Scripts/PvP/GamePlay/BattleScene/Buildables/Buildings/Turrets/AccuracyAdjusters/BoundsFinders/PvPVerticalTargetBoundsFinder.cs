namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders
{
    public class PvPVerticalTargetBoundsFinder : PvPTargetBoundsFinder
    {
        public PvPVerticalTargetBoundsFinder(float targetYMarginInM)
            : base(targetXMarginInM: 0, targetYMarginInM: targetYMarginInM)
        {
        }
    }
}

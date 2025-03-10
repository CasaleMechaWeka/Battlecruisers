namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders
{
    public class VerticalTargetBoundsFinder : TargetBoundsFinder
    {
        public VerticalTargetBoundsFinder(float targetYMarginInM)
            : base(targetXMarginInM: 0, targetYMarginInM: targetYMarginInM)
        {
        }
    }
}

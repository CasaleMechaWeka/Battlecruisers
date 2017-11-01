namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders
{
    public class HorizontalTargetBoundsFinder : TargetBoundsFinder
    {
        public HorizontalTargetBoundsFinder(float targetXMarginInM)
            : base(targetXMarginInM, targetYMarginInM: 0)
        {
        }
    }
}

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public class FighterAngleLimiter : AngleLimiter
    {
        public FighterAngleLimiter()
            : base(minAngle: -45, maxAngle: 45)
        {
        }
    }
}

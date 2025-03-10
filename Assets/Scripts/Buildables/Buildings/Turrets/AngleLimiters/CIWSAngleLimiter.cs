namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public class CIWSAngleLimiter : AngleLimiter
    {
        public CIWSAngleLimiter()
            : base(minAngle: -30, maxAngle: 180)
        {
        }
    }
}

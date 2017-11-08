namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public class AntiAirAngleLimiter : AngleLimiter
    {
        public AntiAirAngleLimiter()
            : base(minAngle: 30, maxAngle: 150)
        {
        }
    }
}

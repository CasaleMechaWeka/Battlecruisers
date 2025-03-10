namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public class MissileFighterAngleLimiter : AngleLimiter
    {
        public MissileFighterAngleLimiter()
            : base(minAngle: -30, maxAngle: 30)
        {
        }
    }
}

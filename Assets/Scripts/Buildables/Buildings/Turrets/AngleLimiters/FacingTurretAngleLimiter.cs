namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public class FacingTurretAngleLimiter : AngleLimiter
    {
        public FacingTurretAngleLimiter() 
            : base(minAngle: -30, maxAngle: 90)
        {
        }
    }
}

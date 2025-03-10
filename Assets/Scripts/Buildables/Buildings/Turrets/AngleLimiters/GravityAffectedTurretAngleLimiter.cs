namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public class GravityAffectedTurretAngleLimiter : AngleLimiter
    {
        public GravityAffectedTurretAngleLimiter() 
            : base(minAngle: -20, maxAngle: 85)
        {
        }
    }
}

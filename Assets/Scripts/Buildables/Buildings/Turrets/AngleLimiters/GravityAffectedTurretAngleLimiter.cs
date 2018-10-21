namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public class GravityAffectedTurretAngleLimiter : AngleLimiter
    {
        public GravityAffectedTurretAngleLimiter() 
            : base(minAngle: 0, maxAngle: 85)
        {
        }
    }
}

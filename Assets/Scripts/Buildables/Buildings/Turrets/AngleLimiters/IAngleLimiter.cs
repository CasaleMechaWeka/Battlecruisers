namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public interface IAngleLimiter
    {
        float LimitAngle(float desiredAngleInDegrees);
    }
}

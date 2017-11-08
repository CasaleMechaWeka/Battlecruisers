namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    /// <summary>
    /// Null object
    /// </summary>
    public class DummyAngleLimiter : IAngleLimiter
    {
        public float LimitAngle(float desiredAngleInDegrees)
        {
            return desiredAngleInDegrees;
        }
    }
}

namespace BattleCruisers.Movement.Velocity
{
    public class StaticVelocityProvider : IVelocityProvider
    {
        public float VelocityInMPerS { get; private set; }

        public StaticVelocityProvider(float velocityInMPerS)
        {
            VelocityInMPerS = velocityInMPerS;
        }
    }
}

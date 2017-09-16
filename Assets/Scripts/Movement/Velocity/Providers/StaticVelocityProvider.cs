namespace BattleCruisers.Movement.Velocity.Providers
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

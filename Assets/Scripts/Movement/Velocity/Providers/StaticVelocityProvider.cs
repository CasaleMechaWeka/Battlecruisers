namespace BattleCruisers.Movement.Velocity.Providers
{
    public class StaticVelocityProvider : IVelocityProvider
    {
        public float VelocityInMPerS { get; }

        public StaticVelocityProvider(float velocityInMPerS)
        {
            VelocityInMPerS = velocityInMPerS;
        }
    }
}

using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity.Providers
{
    public class MultiplyingVelocityProvider : IVelocityProvider
    {
        private readonly IVelocityProvider _providerToWrap;
        private readonly float _multiplier;

        public float VelocityInMPerS => _providerToWrap.VelocityInMPerS * _multiplier;

        public MultiplyingVelocityProvider(IVelocityProvider providerToWrap, float multiplier)
        {
            Assert.IsNotNull(providerToWrap);

            _providerToWrap = providerToWrap;
            _multiplier = multiplier;
        }
    }
}

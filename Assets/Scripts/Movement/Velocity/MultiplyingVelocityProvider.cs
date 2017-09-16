using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity
{
    public class MultiplyingVelocityProvider : IVelocityProvider
    {
        private readonly IVelocityProvider _providerToWrap;
        private readonly float _multiplier;

        public float VelocityInMPerS { get { return _providerToWrap.VelocityInMPerS * _multiplier; } }

        public MultiplyingVelocityProvider(IVelocityProvider providerToWrap, float multiplier)
        {
            Assert.IsNotNull(providerToWrap);

            _providerToWrap = providerToWrap;
            _multiplier = multiplier;
        }
    }
}

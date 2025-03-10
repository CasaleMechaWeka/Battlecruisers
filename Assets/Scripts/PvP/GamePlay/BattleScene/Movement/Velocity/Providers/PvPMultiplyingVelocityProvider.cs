using BattleCruisers.Movement.Velocity.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers
{
    public class PvPMultiplyingVelocityProvider : IVelocityProvider
    {
        private readonly IVelocityProvider _providerToWrap;
        private readonly float _multiplier;

        public float VelocityInMPerS => _providerToWrap.VelocityInMPerS * _multiplier;

        public PvPMultiplyingVelocityProvider(IVelocityProvider providerToWrap, float multiplier)
        {
            Assert.IsNotNull(providerToWrap);

            _providerToWrap = providerToWrap;
            _multiplier = multiplier;
        }
    }
}

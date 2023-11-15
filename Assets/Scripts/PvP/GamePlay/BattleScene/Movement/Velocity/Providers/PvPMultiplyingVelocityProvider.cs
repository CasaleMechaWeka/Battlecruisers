using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers
{
    public class PvPMultiplyingVelocityProvider : IPvPVelocityProvider
    {
        private readonly IPvPVelocityProvider _providerToWrap;
        private readonly float _multiplier;

        public float VelocityInMPerS => _providerToWrap.VelocityInMPerS * _multiplier;

        public PvPMultiplyingVelocityProvider(IPvPVelocityProvider providerToWrap, float multiplier)
        {
            Assert.IsNotNull(providerToWrap);

            _providerToWrap = providerToWrap;
            _multiplier = multiplier;
        }
    }
}

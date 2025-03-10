using BattleCruisers.Movement.Velocity.Providers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers
{
    public class PvPStaticVelocityProvider : IVelocityProvider
    {
        public float VelocityInMPerS { get; }

        public PvPStaticVelocityProvider(float velocityInMPerS)
        {
            VelocityInMPerS = velocityInMPerS;
        }
    }
}

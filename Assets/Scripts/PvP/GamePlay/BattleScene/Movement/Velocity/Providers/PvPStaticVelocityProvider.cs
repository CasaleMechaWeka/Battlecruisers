namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers
{
    public class PvPStaticVelocityProvider : IPvPVelocityProvider
    {
        public float VelocityInMPerS { get; }

        public PvPStaticVelocityProvider(float velocityInMPerS)
        {
            VelocityInMPerS = velocityInMPerS;
        }
    }
}

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPCIWSAngleLimiter : PvPAngleLimiter
    {
        public PvPCIWSAngleLimiter()
            : base(minAngle: -30, maxAngle: 150)
        {
        }
    }
}

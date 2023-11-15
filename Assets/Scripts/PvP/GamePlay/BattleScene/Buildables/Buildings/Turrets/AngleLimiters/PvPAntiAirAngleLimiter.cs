namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPAntiAirAngleLimiter : PvPAngleLimiter
    {
        public PvPAntiAirAngleLimiter()
            : base(minAngle: 30, maxAngle: 150)
        {
        }
    }
}

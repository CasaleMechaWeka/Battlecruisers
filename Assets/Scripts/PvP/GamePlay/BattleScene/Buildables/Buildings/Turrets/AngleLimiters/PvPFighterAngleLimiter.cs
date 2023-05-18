namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPFighterAngleLimiter : PvPAngleLimiter
    {
        public PvPFighterAngleLimiter()
            : base(minAngle: -30, maxAngle: 30)
        {
        }
    }
}

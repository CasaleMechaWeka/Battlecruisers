namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPMissileFighterAngleLimiter : PvPAngleLimiter
    {
        public PvPMissileFighterAngleLimiter()
            : base(minAngle: -30, maxAngle: 30)
        {
        }
    }
}

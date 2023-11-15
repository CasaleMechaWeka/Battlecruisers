namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public interface IPvPAngleLimiterFactory
    {
        IPvPAngleLimiter CreateDummyLimiter();
        IPvPAngleLimiter CreateFacingLimiter();
        IPvPAngleLimiter CreateAntiAirLimiter();
        IPvPAngleLimiter CreateFighterLimiter();
        IPvPAngleLimiter CreateGravityAffectedLimiter();
    }
}

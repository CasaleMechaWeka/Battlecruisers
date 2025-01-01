using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public interface IPvPAngleLimiterFactory
    {
        IAngleLimiter CreateDummyLimiter();
        IAngleLimiter CreateFacingLimiter();
        IAngleLimiter CreateAntiAirLimiter();
        IAngleLimiter CreateFighterLimiter();
        IAngleLimiter CreateMissileFighterLimiter();
        IAngleLimiter CreateGravityAffectedLimiter();
        IAngleLimiter CreateCIWSLimiter();
    }
}

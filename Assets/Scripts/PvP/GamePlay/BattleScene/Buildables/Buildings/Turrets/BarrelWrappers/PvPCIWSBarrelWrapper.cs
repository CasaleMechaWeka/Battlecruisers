using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class PvPCIWSBarrelWrapper : PvPLeadingDirectFireBarrelWrapper
    {
        protected override IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateCIWSLimiter();
        }
    }
}

using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Utils.BattleScene.Update;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class DeathstarBarrelWrapper : DirectFireBarrelWrapper
    {
        protected override IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateDummyLimiter();
        }

        protected override IUpdater ChooseUpdater(IUpdaterProvider updaterProvider)
        {
            return updaterProvider.PerFrameUpdater;
        }
    }
}

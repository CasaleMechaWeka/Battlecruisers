using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetProviders;

namespace BattleCruisers.Targets.Factories
{
    public interface ITargetProviderFactory
    {
        ITargetProvider CreateStaticTargetProvider(ITarget target);
        IBroadcastingTargetProvider CreateShipBlockingEnemyProvider(ITargetDetector enemyDetector, IUnit parentUnit);
        IBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(ITargetDetector friendlyDetector, IUnit parentUnit);
    }
}
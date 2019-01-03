using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetProviders;

namespace BattleCruisers.Targets.Factories
{
    public class TargetProviderFactory : ITargetProviderFactory
    {
        public ITargetProvider CreateStaticTargetProvider(ITarget target)
		{
            return new StaticTargetProvider(target);
		}

        public IBroadcastingTargetProvider CreateShipBlockingEnemyProvider(ITargetDetector enemyDetector, IUnit parentUnit)
        {
            // FELIX  Pass ITargetFactoryProvider :)
            return new ShipBlockingEnemyProvider(null, enemyDetector, parentUnit);
        }

        public IBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(ITargetDetector friendlyDetector, IUnit parentUnit)
        {
            // FELIX  Pass ITargetFactoryProvider :)
            return new ShipBlockingFriendlyProvider(null, friendlyDetector, parentUnit);
        }
    }
}

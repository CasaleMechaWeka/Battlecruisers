using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Targets.Factories
{
    public class TargetProviderFactory : ITargetProviderFactory
    {
        private readonly ITargetFactoriesProvider _targetFactoriesProvider;
        private readonly ICruiserSpecificFactories _cruiserSpecificFactories;

        public TargetProviderFactory(ICruiserSpecificFactories cruiserSpecificFactories, ITargetFactoriesProvider targetFactoriesProvider)
        {
            Helper.AssertIsNotNull(cruiserSpecificFactories, targetFactoriesProvider);

            _cruiserSpecificFactories = cruiserSpecificFactories;
            _targetFactoriesProvider = targetFactoriesProvider;
        }

        public ITargetProvider CreateStaticTargetProvider(ITarget target)
		{
            return new StaticTargetProvider(target);
		}

        public IBroadcastingTargetProvider CreateShipBlockingEnemyProvider(ITargetDetector enemyDetector, IUnit parentUnit)
        {
            return new ShipBlockingEnemyProvider(_cruiserSpecificFactories, _targetFactoriesProvider, enemyDetector, parentUnit);
        }

        public IBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(ITargetDetector friendlyDetector, IUnit parentUnit)
        {
            return new ShipBlockingFriendlyProvider(_targetFactoriesProvider, friendlyDetector, parentUnit);
        }
    }
}

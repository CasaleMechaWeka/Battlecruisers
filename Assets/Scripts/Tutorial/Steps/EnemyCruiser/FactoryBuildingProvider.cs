using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Tutorial.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    // FELIX  Move to providers namespace
    // FELIX  Rename
    public class FactoryBuildingProvider : IProvider<IFactory>
    {
        private readonly IProvider<IBuildable> _buildableProvider;

        public FactoryBuildingProvider(IProvider<IBuildable> buildingProvider)
        {
            Assert.IsNotNull(buildingProvider);
            _buildableProvider = buildingProvider;
        }

        public IFactory FindItem()
        {
            IFactory factory = _buildableProvider.FindItem() as IFactory;
            Assert.IsNotNull(factory);
            return factory;
        }
    }
}

using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Tutorial.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    public class FactoryProvider : IProvider<IFactory>
    {
        private readonly IProvider<IBuilding> _buildingProvider;

        public FactoryProvider(IProvider<IBuilding> buildingProvider)
        {
            Assert.IsNotNull(buildingProvider);
            _buildingProvider = buildingProvider;
        }

        public IFactory FindItem()
        {
            IFactory factory = _buildingProvider.FindItem() as IFactory;
            Assert.IsNotNull(factory);
            return factory;
        }
    }
}

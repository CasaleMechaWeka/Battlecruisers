using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Tutorial.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class BuildableToFactoryProvider : IItemProvider<IFactory>
    {
        private readonly IItemProvider<IBuildable> _buildableProvider;

        public BuildableToFactoryProvider(IItemProvider<IBuildable> buildableProvider)
        {
            Assert.IsNotNull(buildableProvider);
            _buildableProvider = buildableProvider;
        }

        public IFactory FindItem()
        {
            IFactory factory = _buildableProvider.FindItem() as IFactory;
            Assert.IsNotNull(factory);
            return factory;
        }
    }
}

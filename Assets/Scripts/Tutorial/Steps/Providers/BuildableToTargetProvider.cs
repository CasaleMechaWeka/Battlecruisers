using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class BuildableToTargetProvider : IItemProvider<ITarget>
    {
        private readonly IItemProvider<IBuildable> _buildableProvider;

        public BuildableToTargetProvider(IItemProvider<IBuildable> buildableProvider)
        {
            Assert.IsNotNull(buildableProvider);
            _buildableProvider = buildableProvider;
        }

        public ITarget FindItem()
        {
            return _buildableProvider.FindItem();
        }
    }
}

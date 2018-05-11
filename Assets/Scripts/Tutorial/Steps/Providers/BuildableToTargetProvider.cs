using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class BuildableToTargetProvider : IProvider<ITarget>
    {
        private readonly IProvider<IBuildable> _buildableProvider;

        public BuildableToTargetProvider(IProvider<IBuildable> buildableProvider)
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

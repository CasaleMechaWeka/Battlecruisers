using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Steps.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting.Providers
{
    public class LastBuildingStartedHighlightableProvider : IHighlightablesProvider
    {
        private readonly IProvider<IBuildable> _lastBuildingStartedProvider;

        public LastBuildingStartedHighlightableProvider(IProvider<IBuildable> lastBuildingStartedProvider)
        {
            Assert.IsNotNull(lastBuildingStartedProvider);
            _lastBuildingStartedProvider = lastBuildingStartedProvider;
        }

        public IList<IHighlightable> FindHighlightables()
        {
            IHighlightable lastBuildingStarted = _lastBuildingStartedProvider.FindItem();
            Assert.IsNotNull(lastBuildingStarted);

            return new List<IHighlightable>()
            {
                lastBuildingStarted
            };
        }
    }
}

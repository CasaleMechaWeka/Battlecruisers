using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class LastIncompleteBuildingStartedProvider : ISingleBuildableProvider
    {
        // Sorted list of buildings, with the most recently started
        // building at the end.
        private readonly IList<IBuildable> _incompleteBuildables;

        private IBuildable LastIncompleteBuildingStarted
        {
            get
            {
                return _incompleteBuildables.LastOrDefault();
            }
        }

        public LastIncompleteBuildingStartedProvider(ICruiserController cruiser)
        {
            Assert.IsNotNull(cruiser);

            _incompleteBuildables = new List<IBuildable>();

            cruiser.BuildingStarted += cruiser_BuildingStarted;
            cruiser.BuildingCompleted += cruiser_BuildingCompleted;
        }

        private void cruiser_BuildingStarted(object sender, StartedBuildingConstructionEventArgs e)
        {
            _incompleteBuildables.Add(e.Buildable);
        }

        private void cruiser_BuildingCompleted(object sender, CompletedBuildingConstructionEventArgs e)
        {
            _incompleteBuildables.Remove(e.Buildable);
        }

        public IBuildable FindItem()
        {
            return LastIncompleteBuildingStarted;
        }

        IMaskHighlightable IItemProvider<IMaskHighlightable>.FindItem()
        {
            return LastIncompleteBuildingStarted;
        }

        IClickableEmitter IItemProvider<IClickableEmitter>.FindItem()
        {
            return LastIncompleteBuildingStarted;
        }
    }
}

using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
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

        public LastIncompleteBuildingStartedProvider(ICruiserBuildingMonitor playerBuildingMonitor)
        {
            Assert.IsNotNull(playerBuildingMonitor);

            _incompleteBuildables = new List<IBuildable>();

            playerBuildingMonitor.BuildingStarted += playerBuildingMonitor_BuildingStarted;
            playerBuildingMonitor.BuildingCompleted += playerBuildingMonitor_BuildingCompleted;
        }

        private void playerBuildingMonitor_BuildingStarted(object sender, BuildingStartedEventArgs e)
        {
            _incompleteBuildables.Add(e.StartedBuilding);
        }

        private void playerBuildingMonitor_BuildingCompleted(object sender, BuildingCompletedEventArgs e)
        {
            _incompleteBuildables.Remove(e.CompletedBuilding);
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

using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class LastIncompleteBuildingStartedProvider : ISingleBuildableProvider
    {
        // Sorted list of buildables, witht he most recently started
        // buildable at the end.
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

            cruiser.StartedConstruction += cruiser_StartedConstruction;
            cruiser.BuildingCompleted += cruiser_BuildingCompleted;
        }

        private void cruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {
            _incompleteBuildables.Add(e.Buildable);
        }

        private void cruiser_BuildingCompleted(object sender, CompletedConstructionEventArgs e)
        {
            _incompleteBuildables.Remove(e.Buildable);
        }

        IList<IHighlightable> IListProvider<IHighlightable>.FindItems()
        {
            IList<IHighlightable> highlightables = new List<IHighlightable>();

            if (LastIncompleteBuildingStarted != null)
            {
                highlightables.Add(LastIncompleteBuildingStarted);
            }

            return highlightables;
        }

        IList<IClickableEmitter> IListProvider<IClickableEmitter>.FindItems()
        {
            IList<IClickableEmitter> clickables = new List<IClickableEmitter>();

            if (LastIncompleteBuildingStarted != null)
            {
                clickables.Add(LastIncompleteBuildingStarted);
            }

            return clickables;
        }

        public IBuildable FindItem()
        {
            return LastIncompleteBuildingStarted;
        }
    }
}

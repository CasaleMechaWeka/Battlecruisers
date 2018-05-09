using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class LastBuildingStartedProvider : ILastBuildingStartedProvider
    {
        private readonly ICruiserController _cruiser;
        private IBuildable _lastBuildingStarted;

        public LastBuildingStartedProvider(ICruiserController cruiser)
        {
            Assert.IsNotNull(cruiser);
            _cruiser = cruiser;

            _cruiser.StartedConstruction += _cruiser_StartedConstruction;
        }

        private void _cruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {
            _lastBuildingStarted = e.Buildable;
        }

        IList<IHighlightable> IListProvider<IHighlightable>.FindItems()
        {
            IList<IHighlightable> highlightables = new List<IHighlightable>();

            if (_lastBuildingStarted != null)
            {
                highlightables.Add(_lastBuildingStarted);
            }

            return highlightables;
        }

        IList<IClickable> IListProvider<IClickable>.FindItems()
        {
            IList<IClickable> clickables = new List<IClickable>();

            if (_lastBuildingStarted != null)
            {
                clickables.Add(_lastBuildingStarted);
            }

            return clickables;
        }

        public IBuildable FindItem()
        {
            return _lastBuildingStarted;
        }
    }
}

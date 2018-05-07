using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    // FELIX  Test
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

        public IList<IHighlightable> FindHighlightables()
        {
            return new List<IHighlightable>()
            {
                _lastBuildingStarted
            };
        }

        public IList<IClickable> FindClickables()
        {
            return new List<IClickable>()
            {
                _lastBuildingStarted
            };
        }

        public IBuildable FindItem()
        {
            return _lastBuildingStarted;
        }
    }
}

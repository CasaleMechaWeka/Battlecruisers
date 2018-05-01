using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    // FELIX  Test
    public class LastBuildingStartedProvider : IProvider<IBuildable>
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

        public IBuildable FindItem()
        {
            return _lastBuildingStarted;
        }
    }
}

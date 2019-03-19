using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Construction;

namespace BattleCruisers.Tests.AI.FactoryManagers
{
    // FELIX  Remove?
    internal class StartedConstructionEventArgs : BuildingStartedEventArgs
    {
        public StartedConstructionEventArgs(IBuilding building) : base(building)
        {
        }
    }
}
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;

namespace BattleCruisers.Tests.AI.FactoryManagers
{
    internal class StartedConstructionEventArgs : BuildingStartedEventArgs
    {
        public StartedConstructionEventArgs(IBuilding building) : base(building)
        {
        }
    }
}
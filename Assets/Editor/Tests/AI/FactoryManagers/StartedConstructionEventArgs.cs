using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;

namespace BattleCruisers.Tests.AI.FactoryManagers
{
    internal class StartedConstructionEventArgs : StartedBuildingConstructionEventArgs
    {
        public StartedConstructionEventArgs(IBuilding building) : base(building)
        {
        }
    }
}
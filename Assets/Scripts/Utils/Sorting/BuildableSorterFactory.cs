using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.Utils.Sorting
{
    public class BuildableSorterFactory : IBuildableSorterFactory
    {
        public IBuildableSorter<IBuilding> CreateBuildingSorter()
        {
            return new CostAndNameSorter<IBuilding>();
        }

        public IBuildableSorter<IUnit> CreateUnitSorter()
        {
            return new CostAndNameSorter<IUnit>();
        }
    }
}

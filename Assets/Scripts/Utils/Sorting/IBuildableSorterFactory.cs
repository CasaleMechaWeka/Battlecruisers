using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.Utils.Sorting
{
    public interface IBuildableSorterFactory
    {
        IBuildableSorter<IBuilding> CreateBuildingSorter();
        IBuildableSorter<IUnit> CreateUnitSorter();
    }
}

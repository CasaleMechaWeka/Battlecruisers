using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System.Collections.ObjectModel;

namespace BattleCruisers.Cruisers.Construction
{
    public interface IBuildableMonitor
    {
        ReadOnlyCollection<IBuilding> AliveBuildings { get; }
        ReadOnlyCollection<IUnit> AliveUnits { get; }
    }
}
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System.Collections.Generic;

namespace BattleCruisers.Cruisers.Construction
{
    public interface IBuildableMonitor
    {
        IReadOnlyCollection<IBuilding> AliveBuildings { get; }
        IReadOnlyCollection<IUnit> AliveUnits { get; }
    }
}
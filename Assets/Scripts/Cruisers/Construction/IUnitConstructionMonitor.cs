using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Cruisers.Construction
{
    public class UnitDestroyedEventArgs : EventArgs
    {
        public IUnit DestroyedUnit { get; }

        public UnitDestroyedEventArgs(IUnit destroyedUnit)
        {
            DestroyedUnit = destroyedUnit;
        }
    }

    public interface IUnitConstructionMonitor
    {
        // FELIX  Uncomment!!!
        //IReadOnlyCollection<IUnit> AliveUnits { get; }

        // FELIX  Rename to UnitStarted
        event EventHandler<StartedUnitConstructionEventArgs> StartedBuildingUnit;
        // FELIX  Rename to UnitCompleted
        event EventHandler<CompletedUnitConstructionEventArgs> CompletedBuildingUnit;
        //event EventHandler<UnitDestroyedEventArgs> UnitDestroyed;
    }
}
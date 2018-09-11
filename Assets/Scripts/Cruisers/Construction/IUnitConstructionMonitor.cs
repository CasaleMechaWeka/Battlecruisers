using BattleCruisers.Buildables.Buildings.Factories;
using System;

namespace BattleCruisers.Cruisers.Construction
{
    public interface IUnitConstructionMonitor
    {
        event EventHandler<StartedUnitConstructionEventArgs> StartedBuildingUnit;
    }
}
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.DataStrctures;
using System;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    // FELIX  Rename
    public class StartedUnitConstructionEventArgs : BuildableConstructionEventArgs<IUnit>
    {
        public StartedUnitConstructionEventArgs(IUnit unit)
            : base(unit) { }
    }

    // FELIX  Rename
    public class CompletedUnitConstructionEventArgs : BuildableConstructionEventArgs<IUnit>
    {
        public CompletedUnitConstructionEventArgs(IUnit unit)
            : base(unit) { }
    }

    public interface IFactory : IBuilding
    {
		UnitCategory UnitCategory { get; }
        int NumOfDrones { get; }
        IBuildableWrapper<IUnit> UnitWrapper { get; }
        IUnit UnitUnderConstruction { get; }
        IObservableValue<bool> IsUnitPaused { get; }
        LayerMask UnitLayerMask { get; }

        void StartBuildingUnit(IBuildableWrapper<IUnit> unit);
        void StopBuildingUnit();
        void PauseBuildingUnit();
        void ResumeBuildingUnit();

        event EventHandler<StartedUnitConstructionEventArgs> StartedBuildingUnit;
		event EventHandler<CompletedUnitConstructionEventArgs> CompletedBuildingUnit;
        event EventHandler NewUnitChosen;
	}
}

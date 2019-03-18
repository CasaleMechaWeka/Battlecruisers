using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.DataStrctures;
using System;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class UnitStartedEventArgs : BuildableConstructionEventArgs<IUnit>
    {
        public UnitStartedEventArgs(IUnit unit)
            : base(unit) { }
    }

    public class UnitCompletedEventArgs : BuildableConstructionEventArgs<IUnit>
    {
        public UnitCompletedEventArgs(IUnit unit)
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

        event EventHandler<UnitStartedEventArgs> StartedBuildingUnit;
		event EventHandler<UnitCompletedEventArgs> CompletedBuildingUnit;
        event EventHandler NewUnitChosen;
	}
}

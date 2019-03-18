using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.DataStrctures;
using System;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    // FELIX  Move to ICruiserUnitMonitor :)
    public class UnitStartedEventArgs : EventArgs
    {
        public IUnit StartedUnit { get; }

        public UnitStartedEventArgs(IUnit startedUnit)
        {
            StartedUnit = startedUnit;
        }
    }

    public class UnitCompletedEventArgs : EventArgs
    {
        public IUnit CompletedUnit { get; }

        public UnitCompletedEventArgs(IUnit completedUnit)
        {
            CompletedUnit = completedUnit;
        }
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

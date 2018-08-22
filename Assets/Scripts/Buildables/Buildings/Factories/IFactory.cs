using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using System;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class StartedUnitConstructionEventArgs : BuildableConstructionEventArgs<IUnit>
    {
        public StartedUnitConstructionEventArgs(IUnit unit)
            : base(unit) { }
    }

    public class CompletedUnitConstructionEventArgs : BuildableConstructionEventArgs<IUnit>
    {
        public CompletedUnitConstructionEventArgs(IUnit unit)
            : base(unit) { }
    }

    public interface IFactory : IBuilding
    {
		UnitCategory UnitCategory { get; }
        int NumOfDrones { get; }
        bool IsUnitPaused { get; }
        IBuildableWrapper<IUnit> UnitWrapper { get; }
        IUnit UnitUnderConstruction { get; }

        void StartBuildingUnit(IBuildableWrapper<IUnit> unit);
        void StopBuildingUnit();
        void PauseBuildingUnit();
        void ResumeBuildingUnit();

        event EventHandler<StartedUnitConstructionEventArgs> StartedBuildingUnit;
		event EventHandler<CompletedUnitConstructionEventArgs> CompletedBuildingUnit;
        event EventHandler IsUnitPausedChanged;
	}
}

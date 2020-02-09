using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.BuildProgress
{
    // FELIX  test :)
    public class UnitBuildProgressTrigger : IUnitBuildProgressTrigger
    {
        private readonly IUnitBuildProgress _unitBuildProgress;

        private IFactory _factory;

        public UnitBuildProgressTrigger(IUnitBuildProgress unitBuildProgress)
        {
            Assert.IsNotNull(unitBuildProgress);
            _unitBuildProgress = unitBuildProgress;
        }

        public IFactory Factory
        {
            private get => _factory;
            set
            {
                if (_factory != null)
                {
                    _factory.UnitStarted -= _factory_StartedBuildingUnit;
                    _factory.NewUnitChosen -= _factory_NewUnitChosen;
                    _factory.UnitUnderConstructionDestroyed -= _factory_UnitUnderConstructionDestroyed;
                }

                _factory = value;

                if (_factory != null)
                {
                    ShowBuildProgressIfNecessary(_factory.UnitUnderConstruction);

                    _factory.UnitStarted += _factory_StartedBuildingUnit;
                    _factory.NewUnitChosen += _factory_NewUnitChosen;
                    _factory.UnitUnderConstructionDestroyed += _factory_UnitUnderConstructionDestroyed;
                }
            }
        }

        private void _factory_StartedBuildingUnit(object sender, UnitStartedEventArgs e)
        {
            ShowBuildProgressIfNecessary(e.StartedUnit);
        }

        private void _factory_NewUnitChosen(object sender, EventArgs e)
        {
            ShowBuildProgressIfNecessary(_factory.UnitWrapper?.Buildable);
        }

        private void _factory_UnitUnderConstructionDestroyed(object sender, EventArgs e)
        {
            ShowBuildProgressIfNecessary(_factory.UnitWrapper?.Buildable);
        }

        private void ShowBuildProgressIfNecessary(IUnit unit)
        {
            _unitBuildProgress.ShowBuildProgressIfNecessary(unit, _factory);
        }
    }
}
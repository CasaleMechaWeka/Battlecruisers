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
        private readonly string _unitName;
        private readonly IBuildProgressFeedback _buildProgressFeedback;

        private IFactory _factory;
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

        public UnitBuildProgressTrigger(string unitName, IBuildProgressFeedback buildProgressFeedback)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(unitName));
            Assert.IsNotNull(buildProgressFeedback);

            _unitName = unitName;
            _buildProgressFeedback = buildProgressFeedback;
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

        private void ShowBuildProgressIfNecessary(IUnit unitUnderConstruction)
        {
            if (unitUnderConstruction != null
                && unitUnderConstruction.Name == _unitName)
            {
                _buildProgressFeedback.ShowBuildProgress(unitUnderConstruction, _factory);
            }
            else
            {
                _buildProgressFeedback.HideBuildProgress();
            }
        }
    }
}
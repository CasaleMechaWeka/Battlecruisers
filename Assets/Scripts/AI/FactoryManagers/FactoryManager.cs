using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.FactoryManagers
{
    /// <summary>
    /// Monitors all factories for a unit category (air/naval).
    /// 
    /// This manager asks the injected IUnitChooser which unit a factory should
    /// build when that factory:
    /// a) Completes building (itself)
    /// b) Completes building a unit
    /// </summary>
    public class FactoryManager : IFactoryManager
    {
        private readonly UnitCategory _factoryUnitCategory;
        private readonly ICruiserController _friendlyCruiser;
        private readonly IUnitChooser _unitChooser;

        public FactoryManager(UnitCategory factoryUnitCategory, ICruiserController friendlyCruiser, IUnitChooser unitChooser)
        {
            Helper.AssertIsNotNull(friendlyCruiser, unitChooser);

            _factoryUnitCategory = factoryUnitCategory;
            _friendlyCruiser = friendlyCruiser;
            _unitChooser = unitChooser;

            _friendlyCruiser.StartedConstruction += _friendlyCruiser_StartedConstruction;
        }

        private void _friendlyCruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {
            IFactory factory = e.Buildable as IFactory;

            if (factory != null && factory.UnitCategory == _factoryUnitCategory)
            {
                factory.CompletedBuildable += Factory_CompletedBuildable;
                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void Factory_CompletedBuildable(object sender, EventArgs e)
        {
            IFactory factory = ParseSender(sender);

            factory.UnitWrapper = _unitChooser.ChosenUnit;
            factory.CompletedBuildingUnit += Factory_CompletedBuildingUnit;
        }

        private void Factory_CompletedBuildingUnit(object sender, CompletedConstructionEventArgs e)
        {
            IFactory factory = ParseSender(sender);
            factory.UnitWrapper = _unitChooser.ChosenUnit;
        }

        private void Factory_Destroyed(object sender, DestroyedEventArgs e)
        {
            IFactory factory = ParseSender(sender);

            factory.CompletedBuildable -= Factory_CompletedBuildable;
            factory.Destroyed -= Factory_Destroyed;
            factory.CompletedBuildingUnit -= Factory_CompletedBuildingUnit;
        }

        private IFactory ParseSender(object sender)
        {
            IFactory factory = sender as IFactory;
            Assert.IsNotNull(factory);
            return factory;
        }

        public void Dispose()
        {
            _friendlyCruiser.StartedConstruction -= _friendlyCruiser_StartedConstruction;
        }
    }
}

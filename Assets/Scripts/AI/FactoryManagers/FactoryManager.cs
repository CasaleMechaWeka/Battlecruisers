using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
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
    /// c) The unit chooser changes the chosen unit (eg, because the number of drones has changed)
    ///     AND the factory is not building a unit
    /// </summary>
    public class FactoryManager : IFactoryManager
    {
        private readonly HashSet<IFactory> _factories;
        private readonly UnitCategory _factoryUnitCategory;
        private readonly ICruiserController _friendlyCruiser;
        private readonly IUnitChooser _unitChooser;

        public FactoryManager(UnitCategory factoryUnitCategory, ICruiserController friendlyCruiser, IUnitChooser unitChooser)
        {
            Helper.AssertIsNotNull(friendlyCruiser, unitChooser);

            _factoryUnitCategory = factoryUnitCategory;
            _friendlyCruiser = friendlyCruiser;
            _unitChooser = unitChooser;
            _factories = new HashSet<IFactory>();

            _friendlyCruiser.StartedConstruction += _friendlyCruiser_StartedConstruction;
            _unitChooser.ChosenUnitChanged += _unitChooser_ChosenUnitChanged;
        }

        private void _friendlyCruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {
            IFactory factory = e.Buildable as IFactory;

            if (factory != null && factory.UnitCategory == _factoryUnitCategory)
            {
                Assert.IsFalse(_factories.Contains(factory));
                _factories.Add(factory);

                factory.CompletedBuildable += Factory_CompletedBuildable;
                factory.CompletedBuildingUnit += Factory_CompletedBuildingUnit;
                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void Factory_CompletedBuildable(object sender, EventArgs e)
        {
			IFactory factory = sender.Parse<IFactory>();
            Assert.IsTrue(_factories.Contains(factory));

            factory.UnitWrapper = _unitChooser.ChosenUnit;
        }

        private void Factory_CompletedBuildingUnit(object sender, CompletedConstructionEventArgs e)
        {
			IFactory factory = sender.Parse<IFactory>();
            Assert.IsTrue(_factories.Contains(factory));

            factory.UnitWrapper = _unitChooser.ChosenUnit;
        }

        private void Factory_Destroyed(object sender, DestroyedEventArgs e)
        {
            IFactory factory = sender.Parse<IFactory>();
            Assert.IsTrue(_factories.Contains(factory));
            _factories.Remove(factory);

            UnsubscribeFromAllEvents(factory);
        }

        private void UnsubscribeFromAllEvents(IFactory factory)
        {
            factory.CompletedBuildable -= Factory_CompletedBuildable;
            factory.Destroyed -= Factory_Destroyed;
            factory.CompletedBuildingUnit -= Factory_CompletedBuildingUnit;
            _unitChooser.ChosenUnitChanged -= _unitChooser_ChosenUnitChanged;
        }

        private void _unitChooser_ChosenUnitChanged(object sender, EventArgs e)
        {
            foreach (IFactory factory in _factories)
            {
                if (factory.BuildableState == BuildableState.Completed
                    && factory.UnitWrapper == null)
                {
                    factory.UnitWrapper = _unitChooser.ChosenUnit;
                }
            }
        }

        public void Dispose()
        {
            _friendlyCruiser.StartedConstruction -= _friendlyCruiser_StartedConstruction;

            foreach (IFactory factory in _factories)
            {
                UnsubscribeFromAllEvents(factory);
            }

            _factories.Clear();
        }
    }
}

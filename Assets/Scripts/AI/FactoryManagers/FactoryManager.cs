using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.FactoryManagers
{
    /// <summary>
    /// Decides what units all factories should build.
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

            _friendlyCruiser.BuildingStarted += _friendlyCruiser_BuildingStarted;
            _unitChooser.ChosenUnitChanged += _unitChooser_ChosenUnitChanged;
        }

        private void _friendlyCruiser_BuildingStarted(object sender, BuildingStartedEventArgs e)
        {
            IFactory factory = e.StartedBuilding as IFactory;

            if (factory != null && factory.UnitCategory == _factoryUnitCategory)
            {
                Assert.IsFalse(_factories.Contains(factory));
                _factories.Add(factory);

                factory.CompletedBuildable += Factory_CompletedBuildable;
                factory.UnitCompleted += Factory_CompletedBuildingUnit;
                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void Factory_CompletedBuildable(object sender, EventArgs e)
        {
			IFactory factory = sender.Parse<IFactory>();
            Assert.IsTrue(_factories.Contains(factory));

            factory.StartBuildingUnit(_unitChooser.ChosenUnit);
        }

        private void Factory_CompletedBuildingUnit(object sender, UnitCompletedEventArgs e)
        {
			IFactory factory = sender.Parse<IFactory>();
            Assert.IsTrue(_factories.Contains(factory));

            factory.StartBuildingUnit(_unitChooser.ChosenUnit);
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
            factory.UnitCompleted -= Factory_CompletedBuildingUnit;
            _unitChooser.ChosenUnitChanged -= _unitChooser_ChosenUnitChanged;
        }

        private void _unitChooser_ChosenUnitChanged(object sender, EventArgs e)
        {
            foreach (IFactory factory in _factories)
            {
                if (factory.BuildableState == BuildableState.Completed
                    && factory.UnitWrapper == null)
                {
                    factory.StartBuildingUnit(_unitChooser.ChosenUnit);
                }
            }
        }

        public void DisposeManagedState()
        {
            _friendlyCruiser.BuildingStarted -= _friendlyCruiser_BuildingStarted;

            foreach (IFactory factory in _factories)
            {
                UnsubscribeFromAllEvents(factory);
            }

            _factories.Clear();
        }
    }
}

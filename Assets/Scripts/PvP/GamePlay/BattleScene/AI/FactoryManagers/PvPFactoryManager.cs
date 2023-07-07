using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
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
    public class PvPFactoryManager : IPvPFactoryManager
    {
        private readonly HashSet<IPvPFactory> _factories;
        private readonly PvPUnitCategory _factoryUnitCategory;
        private readonly IPvPCruiserController _friendlyCruiser;
        private readonly IPvPUnitChooser _unitChooser;

        public PvPFactoryManager(PvPUnitCategory factoryUnitCategory, IPvPCruiserController friendlyCruiser, IPvPUnitChooser unitChooser)
        {
            PvPHelper.AssertIsNotNull(friendlyCruiser, unitChooser);

            _factoryUnitCategory = factoryUnitCategory;
            _friendlyCruiser = friendlyCruiser;
            _unitChooser = unitChooser;
            _factories = new HashSet<IPvPFactory>();

            _friendlyCruiser.BuildingStarted += _friendlyCruiser_BuildingStarted;
            _unitChooser.ChosenUnitChanged += _unitChooser_ChosenUnitChanged;
        }

        private void _friendlyCruiser_BuildingStarted(object sender, PvPBuildingStartedEventArgs e)
        {
            IPvPFactory factory = e.StartedBuilding as IPvPFactory;

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
            IPvPFactory factory = sender.Parse<IPvPFactory>();
            Assert.IsTrue(_factories.Contains(factory));

            factory.StartBuildingUnit(_unitChooser.ChosenUnit);
        }

        private void Factory_CompletedBuildingUnit(object sender, PvPUnitCompletedEventArgs e)
        {
            IPvPFactory factory = sender.Parse<IPvPFactory>();
            Assert.IsTrue(_factories.Contains(factory));

            factory.StartBuildingUnit(_unitChooser.ChosenUnit);
        }

        private void Factory_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            IPvPFactory factory = sender.Parse<IPvPFactory>();
            Assert.IsTrue(_factories.Contains(factory));
            _factories.Remove(factory);

            UnsubscribeFromAllEvents(factory);
        }

        private void UnsubscribeFromAllEvents(IPvPFactory factory)
        {
            factory.CompletedBuildable -= Factory_CompletedBuildable;
            factory.Destroyed -= Factory_Destroyed;
            factory.UnitCompleted -= Factory_CompletedBuildingUnit;
            _unitChooser.ChosenUnitChanged -= _unitChooser_ChosenUnitChanged;
        }

        private void _unitChooser_ChosenUnitChanged(object sender, EventArgs e)
        {
            foreach (IPvPFactory factory in _factories)
            {
                if (factory.BuildableState == PvPBuildableState.Completed
                    && factory.UnitWrapper == null)
                {
                    factory.StartBuildingUnit(_unitChooser.ChosenUnit);
                }
            }
        }

        public void DisposeManagedState()
        {
            _friendlyCruiser.BuildingStarted -= _friendlyCruiser_BuildingStarted;

            foreach (IPvPFactory factory in _factories)
            {
                UnsubscribeFromAllEvents(factory);
            }

            _factories.Clear();
        }
    }
}

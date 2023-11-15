using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.Strategies;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones
{
    /// <summary>
    /// Manages which drone consumer should be in focus, thus having the most
    /// drones.
    /// 
    /// Ensures the drone consumers of factories that are producing units
    /// (completed factories) are never the highest priority drone
    /// consumer, unless they are:
    /// 1. The only drone consumer
    /// 2. Or, producing the desired number of units (usually 3ish).  This means
    /// factories produce some units (otherwise they have no point :P) but stop 
    /// hogging drones after having produced some units (so that other buildings
    /// can be built).
    /// </summary>
    public class PvPDroneConsumerFocusManager : IPvPManagedDisposable
    {
        private readonly IPvPDroneFocusingStrategy _strategy;
        private readonly IPvPCruiserBuildingMonitor _aiBuildingMonitor;
        private readonly IPvPDroneManager _droneManager;
        private readonly IList<IPvPFactory> _completedFactories;
        private readonly IPvPDroneConsumerFocusHelper _focusHelper;

        public PvPDroneConsumerFocusManager(
            IPvPDroneFocusingStrategy strategy,
            IPvPCruiserController aiCruiser,
            IPvPDroneConsumerFocusHelper focusHelper)
        {
            PvPHelper.AssertIsNotNull(strategy, aiCruiser, aiCruiser.DroneManager, focusHelper);

            _strategy = strategy;
            _aiBuildingMonitor = aiCruiser.BuildingMonitor;
            _droneManager = aiCruiser.DroneManager;
            _focusHelper = focusHelper;

            _completedFactories = new List<IPvPFactory>();

            _aiBuildingMonitor.BuildingStarted += _aiBuildingMonitor_BuildingStarted;
            _aiBuildingMonitor.BuildingCompleted += _aiBuildingMonitor_BuildingCompleted;
        }

        private void _aiBuildingMonitor_BuildingCompleted(object sender, PvPBuildingCompletedEventArgs e)
        {
            IPvPFactory factory = e.CompletedBuilding as IPvPFactory;

            if (factory != null)
            {
                factory.UnitStarted += Factory_StartedBuildingFirstUnit;

                Assert.IsFalse(_completedFactories.Contains(factory));
                _completedFactories.Add(factory);

                factory.UnitStarted += Factory_StartedBuildingUnit;
                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void _aiBuildingMonitor_BuildingStarted(object sender, PvPBuildingStartedEventArgs e)
        {
            if (_strategy.EvaluateWhenBuildingStarted)
            {
                _focusHelper.FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
            }
        }

        /// <summary>
        /// The first time a factory starts building a unit, toggle its drone focus
        /// so it becomes the highest priority drone consumer.  This means it has 
        /// higher priority than previously built factories.
        /// </summary>
        private void Factory_StartedBuildingFirstUnit(object sender, PvPUnitStartedEventArgs e)
        {
            IPvPFactory factory = sender.Parse<IPvPFactory>();
            factory.UnitStarted -= Factory_StartedBuildingFirstUnit;

            Assert.IsNotNull(factory.DroneConsumer);
            _droneManager.ToggleDroneConsumerFocus(factory.DroneConsumer);
        }

        private void Factory_StartedBuildingUnit(object sender, PvPUnitStartedEventArgs e)
        {
        //    Logging.LogMethod(Tags.DRONE_CONUMSER_FOCUS_MANAGER);

            if (_strategy.EvaluateWhenUnitStarted)
            {
                _focusHelper.FocusOnNonFactoryDroneConsumer(_strategy.ForceInProgressBuildingToFocused);
            }
        }

        private void Factory_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            IPvPFactory destroyedFactory = e.DestroyedTarget.Parse<IPvPFactory>();

            Assert.IsTrue(_completedFactories.Contains(destroyedFactory));
            _completedFactories.Remove(destroyedFactory);

            UnsubscribeFromFactoryEvents(destroyedFactory);
        }

        public void DisposeManagedState()
        {
            _aiBuildingMonitor.BuildingStarted -= _aiBuildingMonitor_BuildingStarted;
            _aiBuildingMonitor.BuildingCompleted -= _aiBuildingMonitor_BuildingCompleted;

            foreach (IPvPFactory factory in _completedFactories)
            {
                UnsubscribeFromFactoryEvents(factory);
            }
            _completedFactories.Clear();
        }

        private void UnsubscribeFromFactoryEvents(IPvPFactory factory)
        {
            factory.UnitStarted -= Factory_StartedBuildingUnit;
            factory.UnitStarted -= Factory_StartedBuildingFirstUnit;
            factory.Destroyed -= Factory_Destroyed;
        }
    }
}

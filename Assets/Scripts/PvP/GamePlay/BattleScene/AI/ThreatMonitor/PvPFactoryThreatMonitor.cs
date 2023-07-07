using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    /// <summary>
    /// Monitors the number of drones used by all factories of a specific unit type.
    /// 
    /// Evaluates the threat level when:
    /// + A factory is started
    /// + A factory is completed
    /// + The number of drones used by a factory change
    /// </summary>
    public class PvPFactoryThreatMonitor : PvPImmediateThreatMonitor
    {
        private readonly PvPUnitCategory _threatCategory;
        private readonly IList<IPvPFactory> _factories;

        public PvPFactoryThreatMonitor(IPvPCruiserController enemyCruiser, IPvPThreatEvaluator threatEvaluator, PvPUnitCategory threatCategory)
            : base(enemyCruiser, threatEvaluator)
        {
            _threatCategory = threatCategory;
            _factories = new List<IPvPFactory>();

            _enemyCruiser.BuildingStarted += _enemyCruiser_BuildingStarted;
        }

        private void _enemyCruiser_BuildingStarted(object sender, PvPBuildingStartedEventArgs e)
        {
            IPvPFactory factory = e.StartedBuilding as IPvPFactory;

            if (factory != null
                && factory.UnitCategory == _threatCategory)
            {
                Assert.IsFalse(_factories.Contains(factory));
                _factories.Add(factory);

                factory.Destroyed += Factory_Destroyed;
                factory.DroneNumChanged += Factory_DroneNumChanged;

                EvaluateThreatLevel();
            }
        }

        private void Factory_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= Factory_Destroyed;

            IPvPFactory destroyedFactory = e.DestroyedTarget.Parse<IPvPFactory>();
            Assert.IsTrue(_factories.Contains(destroyedFactory));

            _factories.Remove(destroyedFactory);
            destroyedFactory.DroneNumChanged -= Factory_DroneNumChanged;

            EvaluateThreatLevel();
        }

        private void Factory_DroneNumChanged(object sender, PvPDroneNumChangedEventArgs e)
        {
            EvaluateThreatLevel();
        }

        protected override float FindThreatEvaluationParameter()
        {
            return _factories.Sum(factory => factory.NumOfDrones);
        }
    }
}

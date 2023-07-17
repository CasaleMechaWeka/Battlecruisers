using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors
{
    public class PvPFactoriesMonitor : IPvPFactoriesMonitor, IPvPManagedDisposable
    {
        private readonly IPvPCruiserBuildingMonitor _bulidingMonitor;
        private readonly IPvPFactoryMonitorFactory _monitorFactory;
        private readonly IList<IPvPFactoryMonitor> _completedFactories;

        public IReadOnlyCollection<IPvPFactoryMonitor> CompletedFactories { get; }

        public PvPFactoriesMonitor(IPvPCruiserBuildingMonitor buildingMonitor, IPvPFactoryMonitorFactory monitorFactory)
        {
            PvPHelper.AssertIsNotNull(buildingMonitor, monitorFactory);

            _bulidingMonitor = buildingMonitor;
            _monitorFactory = monitorFactory;
            _completedFactories = new List<IPvPFactoryMonitor>();
            CompletedFactories = new ReadOnlyCollection<IPvPFactoryMonitor>(_completedFactories);

            _bulidingMonitor.BuildingCompleted += _buildingMonitor_BuildingCompleted;
        }

        private void _buildingMonitor_BuildingCompleted(object sender, PvPBuildingCompletedEventArgs e)
        {
            IPvPFactory factory = e.CompletedBuilding as IPvPFactory;

            if (factory != null)
            {
                Assert.IsNull(GetMonitor(factory));
                _completedFactories.Add(_monitorFactory.CreateMonitor(factory));

                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void Factory_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            IPvPFactory destroyedFactory = e.DestroyedTarget.Parse<IPvPFactory>();
            destroyedFactory.Destroyed -= Factory_Destroyed;

            IPvPFactoryMonitor factoryMonitor = GetMonitor(destroyedFactory);
            Assert.IsNotNull(factoryMonitor);
            _completedFactories.Remove(factoryMonitor);
        }

        /// <returns>
        /// The montior for the given factory, or null if no monitor exists.
        /// </returns>
        private IPvPFactoryMonitor GetMonitor(IPvPFactory factory)
        {
            return _completedFactories.FirstOrDefault(monitor => ReferenceEquals(monitor.Factory, factory));
        }

        public void DisposeManagedState()
        {
            foreach (IPvPFactoryMonitor factoryMonitor in _completedFactories)
            {
                factoryMonitor.Factory.Destroyed -= Factory_Destroyed;
            }
            _completedFactories.Clear();

            _bulidingMonitor.BuildingCompleted -= _buildingMonitor_BuildingCompleted;
        }
    }
}

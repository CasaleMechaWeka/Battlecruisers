using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Drones.BuildingMonitors
{
    public class FactoriesMonitor : IFactoriesMonitor, IManagedDisposable
    {
        private readonly ICruiserBuildingMonitor _bulidingMonitor;
        private readonly IFactoryMonitorFactory _monitorFactory;
        private readonly IList<IFactoryMonitor> _completedFactories;

        public IReadOnlyCollection<IFactoryMonitor> CompletedFactories { get; }

        public FactoriesMonitor(ICruiserBuildingMonitor buildingMonitor, IFactoryMonitorFactory monitorFactory)
        {
            Helper.AssertIsNotNull(buildingMonitor, monitorFactory);

            _bulidingMonitor = buildingMonitor;
            _monitorFactory = monitorFactory;
            _completedFactories = new List<IFactoryMonitor>();
            CompletedFactories = new ReadOnlyCollection<IFactoryMonitor>(_completedFactories);

            _bulidingMonitor.BuildingCompleted += _buildingMonitor_BuildingCompleted;
        }

        private void _buildingMonitor_BuildingCompleted(object sender, BuildingCompletedEventArgs e)
        {
            IFactory factory = e.Buildable as IFactory;

            if (factory != null)
            {
                Assert.IsNull(GetMonitor(factory));
                _completedFactories.Add(_monitorFactory.CreateMonitor(factory));

                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void Factory_Destroyed(object sender, DestroyedEventArgs e)
        {
            IFactory destroyedFactory = e.DestroyedTarget.Parse<IFactory>();
            destroyedFactory.Destroyed -= Factory_Destroyed;

            IFactoryMonitor factoryMonitor = GetMonitor(destroyedFactory);
            Assert.IsNotNull(factoryMonitor);
            _completedFactories.Remove(factoryMonitor);
        }

        /// <returns>
        /// The montior for the given factory, or null if no monitor exists.
        /// </returns>
        private IFactoryMonitor GetMonitor(IFactory factory)
        {
            return _completedFactories.FirstOrDefault(monitor => ReferenceEquals(monitor.Factory, factory));
        }

        public void DisposeManagedState()
        {
            foreach (IFactoryMonitor factoryMonitor in _completedFactories)
            {
                factoryMonitor.Factory.Destroyed -= Factory_Destroyed;
            }
            _completedFactories.Clear();

            _bulidingMonitor.BuildingCompleted -= _buildingMonitor_BuildingCompleted;
        }
    }
}

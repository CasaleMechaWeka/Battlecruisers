using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Drones
{
    // FELIX  Use & test :)

    /// <summary>
    /// Completed factories are considered low priority if they have completed
    /// the desired number of buildings.
    /// 
    /// Factory priority:
    /// Under construction                      => High priority
    /// Building desired number of units        => High priority
    /// After building desired number of units  => Low priority
    /// </summary>
    public class FactoriesMonitor : IFactoriesMonitor
    {
        private readonly ICruiserController _cruiser;
        private readonly IFactoryMonitorFactory _monitorFactory;
        private readonly IList<IFactoryMonitor> _completedFactories;

        public bool AreAllFactoriesLowPriority { get { return _completedFactories.All(factoryMonitor => factoryMonitor.HasFactoryBuiltDesiredNumOfUnits); } }
        public bool AreAnyFactoriesCompleted { get { return _completedFactories.Count != 0; } }

        public FactoriesMonitor(ICruiserController cruiser, IFactoryMonitorFactory monitorFactory)
        {
            Helper.AssertIsNotNull(cruiser, monitorFactory);

            _cruiser = cruiser;
            _monitorFactory = monitorFactory;
            _completedFactories = new List<IFactoryMonitor>();

            _cruiser.BuildingCompleted += _cruiser_BuildingCompleted;
        }

        private void _cruiser_BuildingCompleted(object sender, CompletedConstructionEventArgs e)
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

            _cruiser.BuildingCompleted -= _cruiser_BuildingCompleted;
        }
    }
}

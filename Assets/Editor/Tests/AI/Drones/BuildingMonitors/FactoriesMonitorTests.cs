using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Tests.Utils.Extensions;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.Drones.BuildingMonitors
{
    public class FactoriesMonitorTests
    {
        private FactoriesMonitor _monitor;
        private ICruiserController _cruiser;
        private IFactoryMonitorFactory _monitorFactory;
        private IFactoryMonitor _factoryMonitor;
        private IFactory _factory;

        [SetUp]
        public void TestSetup()
        {
            _cruiser = Substitute.For<ICruiserController>();
            _monitorFactory = Substitute.For<IFactoryMonitorFactory>();

            _monitor = new FactoriesMonitor(_cruiser, _monitorFactory);

            _factoryMonitor = Substitute.For<IFactoryMonitor>();

            _factory = Substitute.For<IFactory>();
            _factoryMonitor.Factory.Returns(_factory);

            _monitorFactory.CreateMonitor(_factory).Returns(_factoryMonitor);

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void Default_Empty()
        {
            Assert.AreEqual(0, _monitor.CompletedFactories.Count);
        }

        [Test]
        public void BuildingCompleted_NonFactory_DoesNotAdd()
        {
            IBuilding nonFactoryBuilding = Substitute.For<IBuilding>();
            _cruiser.StartConstructingBuilding(nonFactoryBuilding);

            Assert.AreEqual(0, _monitor.CompletedFactories.Count);
        }

        [Test]
        public void BuildingCompleted_Factory_Adds()
        {
            _cruiser.StartConstructingBuilding(_factory);

            Assert.AreEqual(1, _monitor.CompletedFactories.Count);
            Assert.AreSame(_factoryMonitor, _monitor.CompletedFactories[0]);
        }

        [Test]
        public void BuildingCompleted_DuplicateFactory_Throws()
        {
            _cruiser.StartConstructingBuilding(_factory);
            Assert.Throws<UnityAsserts.AssertionException>(() => _cruiser.StartConstructingBuilding(_factory));
        }

        [Test]
        public void FactoryDestroyed_Removes()
        {
            _cruiser.StartConstructingBuilding(_factory);
            _factory.Destroyed += Raise.EventWith(new DestroyedEventArgs(_factory));
            Assert.AreEqual(0, _monitor.CompletedFactories.Count);
        }

        [Test]
        public void Dispose_RemovesFactories()
        {
            _cruiser.StartConstructingBuilding(_factory);
            _monitor.DisposeManagedState();
            Assert.AreEqual(0, _monitor.CompletedFactories.Count);
        }
    }
}
using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Tests.Utils.Extensions;
using NSubstitute;
using NUnit.Framework;
using System.Linq;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.Drones.BuildingMonitors
{
    public class FactoriesMonitorTests
    {
        private FactoriesMonitor _monitor;
        private ICruiserBuildingMonitor _buildingMonitor;
        private IFactoryMonitorFactory _monitorFactory;
        private IFactoryMonitor _factoryMonitor;
        private IFactory _factory;

        [SetUp]
        public void TestSetup()
        {
            _buildingMonitor = Substitute.For<ICruiserBuildingMonitor>();
            _monitorFactory = Substitute.For<IFactoryMonitorFactory>();

            _monitor = new FactoriesMonitor(_buildingMonitor, _monitorFactory);

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
            _buildingMonitor.CompleteConstructingBuliding(nonFactoryBuilding);

            Assert.AreEqual(0, _monitor.CompletedFactories.Count);
        }

        [Test]
        public void BuildingCompleted_Factory_Adds()
        {
            _buildingMonitor.CompleteConstructingBuliding(_factory);

            Assert.AreEqual(1, _monitor.CompletedFactories.Count);
            Assert.AreSame(_factoryMonitor, _monitor.CompletedFactories.First());
        }

        [Test]
        public void BuildingCompleted_DuplicateFactory_Throws()
        {
            _buildingMonitor.CompleteConstructingBuliding(_factory);
            Assert.Throws<UnityAsserts.AssertionException>(() => _buildingMonitor.CompleteConstructingBuliding(_factory));
        }

        [Test]
        public void FactoryDestroyed_Removes()
        {
            _buildingMonitor.CompleteConstructingBuliding(_factory);
            _factory.Destroyed += Raise.EventWith(new DestroyedEventArgs(_factory));
            Assert.AreEqual(0, _monitor.CompletedFactories.Count);
        }

        [Test]
        public void Dispose_RemovesFactories()
        {
            _buildingMonitor.CompleteConstructingBuliding(_factory);
            _monitor.DisposeManagedState();
            Assert.AreEqual(0, _monitor.CompletedFactories.Count);
        }
    }
}
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.UI.BattleScene.InGameHints;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.InGameHints
{
    public class FactoryMonitorTests
    {
        private IFactoryMonitor _factoryMonitor;
        private ICruiserBuildingMonitor _buildingMonitor;
        private IBuilding _nonFactory;
        private IFactory _factory;
        private int _factoryCompletedCount, _unitChosenCount;

        [SetUp]
        public void TestSetup()
        {
            _buildingMonitor = Substitute.For<ICruiserBuildingMonitor>();
            _factoryMonitor = new FactoryMonitor(_buildingMonitor);

            _factoryCompletedCount = 0;
            _factoryMonitor.FactoryCompleted += (sender, e) => _factoryCompletedCount++;

            _unitChosenCount = 0;
            _factoryMonitor.UnitChosen += (sender, e) => _unitChosenCount++;

            _nonFactory = Substitute.For<IBuilding>();
            _factory = Substitute.For<IFactory>();
        }

        [Test]
        public void _buildingMonitor_BuildingCompleted_NotFactory()
        {
            _buildingMonitor.BuildingCompleted += Raise.EventWith(new BuildingCompletedEventArgs(_nonFactory));
            Assert.AreEqual(0, _factoryCompletedCount);
        }

        [Test]
        public void _buildingMonitor_BuildingCompleted_Factory()
        {
            _buildingMonitor.BuildingCompleted += Raise.EventWith(new BuildingCompletedEventArgs(_factory));
            Assert.AreEqual(1, _factoryCompletedCount);
        }

        [Test]
        public void Factory_NewUnitChosen()
        {
            _buildingMonitor.BuildingCompleted += Raise.EventWith(new BuildingCompletedEventArgs(_factory));
            _factory.NewUnitChosen += Raise.Event();
            Assert.AreEqual(1, _unitChosenCount);
        }

        [Test]
        public void Factory_Destroyed()
        {
            _buildingMonitor.BuildingCompleted += Raise.EventWith(new BuildingCompletedEventArgs(_factory));

            _factory.Destroyed += Raise.EventWith(new DestroyedEventArgs(_factory));

            _factory.NewUnitChosen += Raise.Event();
            Assert.AreEqual(0, _unitChosenCount);
        }
    }
}
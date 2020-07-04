using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.UI.BattleScene.InGameHints;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.InGameHints
{
    public class BuildingMonitorTests
    {
        private IBuildingMonitor _monitor;
        private ICruiserController _cruiser;
        private IBuilding _airFactory, _navalFactory, _building;

        [SetUp]
        public void TestSetup()
        {
            _cruiser = Substitute.For<ICruiserController>();
            _monitor = new BuildingMonitor(_cruiser);

            _airFactory = new AirFactory();
            _navalFactory = new NavalFactory();

            _building = Substitute.For<IBuilding>();
        }

        [Test]
        public void BuildingStarted_AirFactory()
        {
            int eventCount = 0;
            _monitor.AirFactoryStarted += (sender, e) => eventCount++;

            _cruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_airFactory));

            Assert.AreEqual(1, eventCount);
        }
        
        [Test]
        public void BuildingStarted_NavalFactory()
        {
            int eventCount = 0;
            _monitor.NavalFactoryStarted += (sender, e) => eventCount++;

            _cruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_navalFactory));

            Assert.AreEqual(1, eventCount);
        }
        
        [Test]
        public void BuildingStarted_Offensive()
        {
            int eventCount = 0;
            _monitor.OffensiveStarted += (sender, e) => eventCount++;
            _building.Category.Returns(BuildingCategory.Offence);

            _cruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_building));

            Assert.AreEqual(1, eventCount);
        }

        [Test]
        public void BuildingStarted_AirDefensive()
        {
            int eventCount = 0;
            _monitor.AirDefensiveStarted += (sender, e) => eventCount++;
            _building.Category.Returns(BuildingCategory.Defence);
            _building.SlotSpecification.BuildingFunction.Returns(BuildingFunction.AntiAir);

            _cruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_building));

            Assert.AreEqual(1, eventCount);
        }

        [Test]
        public void BuildingStarted_ShipDefensive()
        {
            int eventCount = 0;
            _monitor.ShipDefensiveStarted += (sender, e) => eventCount++;
            _building.Category.Returns(BuildingCategory.Defence);
            _building.SlotSpecification.BuildingFunction.Returns(BuildingFunction.AntiShip);

            _cruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_building));

            Assert.AreEqual(1, eventCount);
        }

        [Test]
        public void BuildingStarted_Shield()
        {
            int eventCount = 0;
            _monitor.ShieldStarted += (sender, e) => eventCount++;
            _building.SlotSpecification.BuildingFunction.Returns(BuildingFunction.Shield);

            _cruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_building));

            Assert.AreEqual(1, eventCount);
        }
    }
}
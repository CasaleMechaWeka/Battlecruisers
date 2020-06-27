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
        private ICruiserController _aiCruiser;
        private IBuilding _airFactory, _navalFactory, _offensive;

        [SetUp]
        public void TestSetup()
        {
            _aiCruiser = Substitute.For<ICruiserController>();
            _monitor = new BuildingMonitor(_aiCruiser);

            _airFactory = new AirFactory();
            _navalFactory = new NavalFactory();

            _offensive = Substitute.For<IBuilding>();
            _offensive.Category.Returns(BuildingCategory.Offence);
        }

        [Test]
        public void BuildingStarted_AirFactory()
        {
            int eventCount = 0;
            _monitor.AirFactoryStarted += (sender, e) => eventCount++;

            _aiCruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_airFactory));

            Assert.AreEqual(1, eventCount);
        }
        
        [Test]
        public void BuildingStarted_NavalFactory()
        {
            int eventCount = 0;
            _monitor.NavalFactoryStarted += (sender, e) => eventCount++;

            _aiCruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_navalFactory));

            Assert.AreEqual(1, eventCount);
        }
        
        [Test]
        public void BuildingStarted_Offensive()
        {
            int eventCount = 0;
            _monitor.OffensiveStarted += (sender, e) => eventCount++;

            _aiCruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_offensive));

            Assert.AreEqual(1, eventCount);
        }
    }
}
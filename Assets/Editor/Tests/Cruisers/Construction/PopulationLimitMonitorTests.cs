using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Construction
{
    public class PopulationLimitMonitorTests
    {
        private IPopulationLimitMonitor _populationLimitMonitor;
        private ICruiserUnitMonitor _unitMonitor;

        [SetUp]
        public void TestSetup()
        {
            _unitMonitor = Substitute.For<ICruiserUnitMonitor>();
            _populationLimitMonitor = new PopulationLimitMonitor(_unitMonitor);
        }

        [Test]
        public void InitialState()
        {
            Assert.IsFalse(_populationLimitMonitor.IsPopulationLimitReached.Value);
        }

        [Test]
        public void UnitCompleted_PopulationLimitReached()
        {
            _unitMonitor.AliveUnits.Count.Returns(Constants.POPULATION_LIMIT);
            _unitMonitor.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(null));
            Assert.IsTrue(_populationLimitMonitor.IsPopulationLimitReached.Value);
        }

        [Test]
        public void UnitCompleted_PopulationLimitNotReached()
        {
            _unitMonitor.AliveUnits.Count.Returns(Constants.POPULATION_LIMIT - 1);
            _unitMonitor.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(null));
            Assert.IsFalse(_populationLimitMonitor.IsPopulationLimitReached.Value);
        }

        [Test]
        public void UnitDestroyed_PopulationLimitReached()
        {
            _unitMonitor.AliveUnits.Count.Returns(Constants.POPULATION_LIMIT);
            _unitMonitor.UnitDestroyed += Raise.EventWith(new UnitDestroyedEventArgs(null));
            Assert.IsTrue(_populationLimitMonitor.IsPopulationLimitReached.Value);
        }

        [Test]
        public void UnitDestroyed_PopulationLimitNotReached()
        {
            _unitMonitor.AliveUnits.Count.Returns(Constants.POPULATION_LIMIT - 1);
            _unitMonitor.UnitDestroyed += Raise.EventWith(new UnitDestroyedEventArgs(null));
            Assert.IsFalse(_populationLimitMonitor.IsPopulationLimitReached.Value);
        }
    }
}
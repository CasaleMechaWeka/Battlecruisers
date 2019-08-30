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
        private int _populationLimitReachedCount;

        [SetUp]
        public void TestSetup()
        {
            _unitMonitor = Substitute.For<ICruiserUnitMonitor>();
            _populationLimitMonitor = new PopulationLimitMonitor(_unitMonitor);

            _populationLimitReachedCount = 0;
            _populationLimitMonitor.PopulationLimitReached += (sender, e) => _populationLimitReachedCount++;
        }

        [Test]
        public void IsPopulationLimitReached_True()
        {
            _unitMonitor.AliveUnits.Count.Returns(Constants.POPULATION_LIMIT);
            Assert.IsTrue(_populationLimitMonitor.IsPopulationLimitReached);
        }

        [Test]
        public void IsPopulationLimitReached_False()
        {
            _unitMonitor.AliveUnits.Count.Returns(Constants.POPULATION_LIMIT - 1);
            Assert.IsFalse(_populationLimitMonitor.IsPopulationLimitReached);
        }

        [Test]
        public void UnitCompleted_PopulationLimitReached_EmitsEvent()
        {
            _unitMonitor.AliveUnits.Count.Returns(Constants.POPULATION_LIMIT);
            _unitMonitor.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(null));
            Assert.AreEqual(1, _populationLimitReachedCount);
        }

        [Test]
        public void UnitCompleted_PopulationLimitNotReached_DoesNotEmitEvent()
        {
            _unitMonitor.AliveUnits.Count.Returns(Constants.POPULATION_LIMIT - 1);
            _unitMonitor.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(null));
            Assert.AreEqual(0, _populationLimitReachedCount);
        }
    }
}
using BattleCruisers.AI.Drones;
using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Tests.AI.Drones
{
    public class FactoryAnalyzerTests
    {
        private FactoryAnalyzer _analyzer;
        private FactoriesMonitor _factoriesMonitor;
        private IList<FactoryMonitor> _factoryMonitors;
        private FactoryMonitor _factoryMonitor;
        private IFilter<FactoryMonitor> _wastingDronesFilter;

        [SetUp]
        public void TestSetup()
        {
            _factoriesMonitor = Substitute.For<FactoriesMonitor>();
            _factoryMonitor = Substitute.For<FactoryMonitor>();
            _wastingDronesFilter = Substitute.For<IFilter<FactoryMonitor>>();

            _factoryMonitors = new List<FactoryMonitor>();
            ReadOnlyCollection<FactoryMonitor> readonlyFactoryMonitors = new ReadOnlyCollection<FactoryMonitor>(_factoryMonitors);
            _factoriesMonitor.CompletedFactories.Returns(readonlyFactoryMonitors);

            _analyzer = new FactoryAnalyzer(_factoriesMonitor, _wastingDronesFilter);
        }

        [Test]
        public void AreAnyFactoriesWronglyUsingDrones_NoFactories_ReturnsFalse()
        {
            Assert.IsFalse(_analyzer.AreAnyFactoriesWronglyUsingDrones);
        }

        [Test]
        public void AreAnyFactoriesWronglyUsingDrones_NoMatchingFactories_ReturnsFalse()
        {
            _factoryMonitors.Add(_factoryMonitor);
            _wastingDronesFilter.IsMatch(_factoryMonitor).Returns(false);

            Assert.IsFalse(_analyzer.AreAnyFactoriesWronglyUsingDrones);
        }

        [Test]
        public void AreAnyFactoriesWronglyUsingDrones_MatchingFactory_ReturnsTrue()
        {
            _factoryMonitors.Add(_factoryMonitor);
            _wastingDronesFilter.IsMatch(_factoryMonitor).Returns(true);

            Assert.IsTrue(_analyzer.AreAnyFactoriesWronglyUsingDrones);
        }
    }
}
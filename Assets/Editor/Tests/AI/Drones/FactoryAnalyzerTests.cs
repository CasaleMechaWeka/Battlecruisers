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
        private IFactoryAnalyzer _analyzer;
        private IFactoriesMonitor _factoriesMonitor;
        private IList<IFactoryMonitor> _factoryMonitors;
        private IFactoryMonitor _factoryMonitor;
        private IFilter<IFactoryMonitor> _wastingDronesFilter;

        [SetUp]
        public void TestSetup()
        {
            _factoriesMonitor = Substitute.For<IFactoriesMonitor>();
            _factoryMonitor = Substitute.For<IFactoryMonitor>();
            _wastingDronesFilter = Substitute.For<IFilter<IFactoryMonitor>>();

            _factoryMonitors = new List<IFactoryMonitor>();
            ReadOnlyCollection<IFactoryMonitor> readonlyFactoryMonitors = new ReadOnlyCollection<IFactoryMonitor>(_factoryMonitors);
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
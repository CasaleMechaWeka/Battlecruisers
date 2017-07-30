using BattleCruisers.AI.ThreatAnalysers;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI
{
    public class FactoryThreatAnalyserTests
    {
        private IThreatAnalyser _threatAnalyzer;
        private ICruiserController _cruiser;
        private IFactory _threateningFactory, _nonThreateningFactory;
        private IBuilding _nonFactoryBuilding;
        private UnitCategory _threatCategory, _nonThreatCategory;

        [SetUp]
        public void SetuUp()
        {
            _cruiser = Substitute.For<ICruiserController>();
            _threatAnalyzer = new FactoryThreatAnalyzer(_cruiser, _threatCategory);

            _threateningFactory = Substitute.For<IFactory>();
            _threateningFactory.NumOfDrones.Returns(7);
            _threateningFactory.UnitCategory.Returns(_threatCategory);

            _nonThreateningFactory = Substitute.For<IFactory>();
            _nonThreateningFactory.UnitCategory.Returns(_nonThreatCategory);
        }

        [Test]
        public void InitialThreatLevel()
        {
            Assert.AreEqual(ThreatLevel.None, _threatAnalyzer.CurrentThreatLevel);
        }
    }
}

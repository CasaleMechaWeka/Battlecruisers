using BattleCruisers.UI.BattleScene.InGameHints;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.InGameHints
{
    public class HintManagerTests
    {
        private HintManager _manager;
        private IBuildingMonitor _buildingMonitor;
        private IHintDisplayer _hintDisplayer;

        [SetUp]
        public void TestSetup()
        {
            _buildingMonitor = Substitute.For<IBuildingMonitor>();
            _hintDisplayer = Substitute.For<IHintDisplayer>();

            _manager = new HintManager(_buildingMonitor, _hintDisplayer);
        }

        [Test]
        public void AirFactoryStarted()
        {
            _buildingMonitor.AirFactoryStarted += Raise.Event();
            _hintDisplayer.Received().ShowHint(Hints.AIR_FACTORY_RESPONSE_HINT);
        }

        [Test]
        public void NavalFactoryStarted()
        {
            _buildingMonitor.NavalFactoryStarted += Raise.Event();
            _hintDisplayer.Received().ShowHint(Hints.NAVAL_FACTORY_RESPONSE_HINT);
        }

        [Test]
        public void OffensiveStarted()
        {
            _buildingMonitor.OffensiveStarted += Raise.Event();
            _hintDisplayer.Received().ShowHint(Hints.OFFENSIVE_RESPONSE_HINT);
        }
    }
}
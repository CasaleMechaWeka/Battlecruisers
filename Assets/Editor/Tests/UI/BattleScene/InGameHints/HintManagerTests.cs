using BattleCruisers.UI.BattleScene.InGameHints;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.InGameHints
{
    public class HintManagerTests
    {
        private HintManager _manager;
        private IBuildingMonitor _enemyBuildingMonitor;
        private IFactoryMonitor _friendlyFactoryMonitor;
        private IHintDisplayer _hintDisplayer;

        [SetUp]
        public void TestSetup()
        {
            _enemyBuildingMonitor = Substitute.For<IBuildingMonitor>();
            _friendlyFactoryMonitor = Substitute.For<IFactoryMonitor>();
            _hintDisplayer = Substitute.For<IHintDisplayer>();

            _manager = new HintManager(_enemyBuildingMonitor, _friendlyFactoryMonitor, _hintDisplayer);
        }

        [Test]
        public void AirFactoryStarted()
        {
            _enemyBuildingMonitor.AirFactoryStarted += Raise.Event();
            _hintDisplayer.Received().ShowHint(Hints.AIR_FACTORY_RESPONSE_HINT);
        }

        [Test]
        public void NavalFactoryStarted()
        {
            _enemyBuildingMonitor.NavalFactoryStarted += Raise.Event();
            _hintDisplayer.Received().ShowHint(Hints.NAVAL_FACTORY_RESPONSE_HINT);
        }

        [Test]
        public void OffensiveStarted()
        {
            _enemyBuildingMonitor.OffensiveStarted += Raise.Event();
            _hintDisplayer.Received().ShowHint(Hints.OFFENSIVE_RESPONSE_HINT);
        }

        [Test]
        public void FactoryCompleted()
        {
            _friendlyFactoryMonitor.FactoryCompleted += Raise.Event();
            _hintDisplayer.Received().ShowHint(Hints.FACTORY_COMPLETED_HINT);
        }

        [Test]
        public void UnitChosen()
        {
            _friendlyFactoryMonitor.UnitChosen += Raise.Event();
            _hintDisplayer.Received().ShowHint(Hints.UNIT_CHOSEN_HINT);
        }
    }
}
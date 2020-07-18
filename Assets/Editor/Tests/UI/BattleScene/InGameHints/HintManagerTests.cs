using BattleCruisers.UI.BattleScene.InGameHints;
using BattleCruisers.Utils.BattleScene;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.InGameHints
{
    public class HintManagerTests
    {
        private HintManager _manager;
        private IBuildingMonitor _enemyBuildingMonitor, _friendlyBuildingMonitor;
        private IFactoryMonitor _friendlyFactoryMonitor;
        private IGameEndMonitor _gameEndMonitor;
        private IHintDisplayer _hintDisplayer;

        [SetUp]
        public void TestSetup()
        {
            _enemyBuildingMonitor = Substitute.For<IBuildingMonitor>();
            _friendlyBuildingMonitor = Substitute.For<IBuildingMonitor>();
            _friendlyFactoryMonitor = Substitute.For<IFactoryMonitor>();
            _gameEndMonitor = Substitute.For<IGameEndMonitor>();
            _hintDisplayer = Substitute.For<IHintDisplayer>();

            // FELIX  Fix :)
            _manager 
                = new HintManager(
                    _enemyBuildingMonitor, 
                    _friendlyBuildingMonitor, 
                    _friendlyFactoryMonitor, 
                    null, 
                    null, 
                    _gameEndMonitor, 
                    _hintDisplayer);
        }

        [Test]
        public void AirFactoryStarted()
        {
            _enemyBuildingMonitor.AirFactoryStarted += Raise.Event();
            _hintDisplayer.Received().ShowHint(Hints.AIR_FACTORY_RESPONSE_HINT);
        }

        [Test]
        public void FriedndlyAirDefensiveStarted()
        {
            _friendlyBuildingMonitor.AirDefensiveStarted+= Raise.Event();
            _hintDisplayer.Received().HideHint(Hints.AIR_FACTORY_RESPONSE_HINT);
        }

        [Test]
        public void NavalFactoryStarted()
        {
            _enemyBuildingMonitor.NavalFactoryStarted += Raise.Event();
            _hintDisplayer.Received().ShowHint(Hints.NAVAL_FACTORY_RESPONSE_HINT);
        }

        [Test]
        public void FriedndlyShipDefensiveStarted()
        {
            _friendlyBuildingMonitor.ShipDefensiveStarted += Raise.Event();
            _hintDisplayer.Received().HideHint(Hints.NAVAL_FACTORY_RESPONSE_HINT);
        }

        [Test]
        public void OffensiveStarted()
        {
            _enemyBuildingMonitor.OffensiveStarted += Raise.Event();
            _hintDisplayer.Received().ShowHint(Hints.OFFENSIVE_RESPONSE_HINT);
        }

        [Test]
        public void FriendlyShieldStarted()
        {
            _friendlyBuildingMonitor.ShieldStarted += Raise.Event();
            _hintDisplayer.Received().HideHint(Hints.OFFENSIVE_RESPONSE_HINT);
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

        [Test]
        public void GameEnded()
        {
            _gameEndMonitor.GameEnded += Raise.Event();
            _hintDisplayer.Received().HideAllHints();
        }
    }
}
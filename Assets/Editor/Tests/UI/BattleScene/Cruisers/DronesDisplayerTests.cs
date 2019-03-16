using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Cruisers
{
    public class DronesDisplayerTests
    {
        private DronesDisplayer _dronesDisplayer;
        private IDroneManager _droneManager;
        private IDroneManagerMonitor _droneManagerMonitor;
        private INumberDisplay _numberDisplay;
        private IGameObject _idleFeedback;

        [SetUp]
        public void TestSetup()
        {
            _droneManager = Substitute.For<IDroneManager>();
            _droneManager.NumOfDrones = 17;

            _droneManagerMonitor = Substitute.For<IDroneManagerMonitor>();
            _numberDisplay = Substitute.For<INumberDisplay>();
            _idleFeedback = Substitute.For<IGameObject>();

            _dronesDisplayer = new DronesDisplayer(_droneManager, _droneManagerMonitor, _numberDisplay, _idleFeedback);

            _numberDisplay.Received().Num = _droneManager.NumOfDrones;
        }

        [Test]
        public void DroneNumChanged_UpdatesNumberDisplay()
        {
            _droneManager.NumOfDrones.Returns(71);
            _droneManager.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(default));

            _numberDisplay.Received().Num = _droneManager.NumOfDrones;
        }

        [Test]
        public void IdleDronesStarted_ShowsIdleFeedback()
        {
            _droneManagerMonitor.IdleDronesStarted += Raise.Event();
            _idleFeedback.Received().IsVisible = true;
        }

        [Test]
        public void IdleDronesEnded_ShowsIdleFeedback()
        {
            _droneManagerMonitor.IdleDronesEnded += Raise.Event();
            _idleFeedback.Received().IsVisible = false;
        }
    }
}
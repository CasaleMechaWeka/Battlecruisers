using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Drones
{
    public class DroneEventSoundPlayerTests
    {
#pragma warning disable CS0414  // Variable is assigned but never used
        private DroneEventSoundPlayer _droneEventSoundPlayer;
#pragma warning restore CS0414  // Variable is assigned but never used
        private IDroneManagerMonitor _droneManagerMonitor;
        private IPrioritisedSoundPlayer _soundPlayer;

        [SetUp]
        public void TestSetup()
        {
            _droneManagerMonitor = Substitute.For<IDroneManagerMonitor>();
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();
            //FELIX   Fix ;)
            _droneEventSoundPlayer = new DroneEventSoundPlayer(_droneManagerMonitor, _soundPlayer, null);
        }

        [Test]
        public void DroneNumberIncrease_PlaysSound()
        {
            _droneManagerMonitor.DroneNumIncreased += Raise.Event();
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Drones.NewDronesReady);
        }

        [Test]
        public void IdleDrones_PlaysSound()
        {
            _droneManagerMonitor.IdleDronesStarted += Raise.Event();
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Drones.Idle);
        }
    }
}
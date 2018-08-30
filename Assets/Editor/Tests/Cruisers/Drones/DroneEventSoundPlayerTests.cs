using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Drones
{
    public class DroneEventSoundPlayerTests
    {
        private DroneEventSoundPlayer _droneEventSoundPlayer;
        private IDroneManagerMonitor _droneManagerMonitor;
        private ISoundPlayer _soundPlayer;

        [SetUp]
        public void TestSetup()
        {
            _droneManagerMonitor = Substitute.For<IDroneManagerMonitor>();
            _soundPlayer = Substitute.For<ISoundPlayer>();
            _droneEventSoundPlayer = new DroneEventSoundPlayer(_droneManagerMonitor, _soundPlayer);
        }

        [Test]
        public void DroneNumberIncrease_PlaysSound()
        {
            _droneManagerMonitor.DroneNumIncreased += Raise.Event();
            _soundPlayer.Received().PlaySound(SoundKeys.Events.DronesNewDronesReady);
        }

        [Test]
        public void IdleDrones_PlaysSound()
        {
            _droneManagerMonitor.IdleDrones += Raise.Event();
            _soundPlayer.Received().PlaySound(SoundKeys.Events.DronesIdle);
        }
    }
}
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Timers;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.Cruisers.Drones
{
    public class DroneEventSoundPlayerTests
    {
#pragma warning disable CS0414  // Variable is assigned but never used
        private DroneEventSoundPlayer _droneEventSoundPlayer;
#pragma warning restore CS0414  // Variable is assigned but never used
        private IDroneManagerMonitor _droneManagerMonitor;
        private IPrioritisedSoundPlayer _soundPlayer;
        private IDebouncer _debouncer;

        [SetUp]
        public void TestSetup()
        {
            _droneManagerMonitor = Substitute.For<IDroneManagerMonitor>();
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();
            _debouncer = Substitute.For<IDebouncer>();

            _droneEventSoundPlayer = new DroneEventSoundPlayer(_droneManagerMonitor, _soundPlayer, _debouncer);
        }

        [Test]
        public void DroneNumberIncrease_PlaysSound()
        {
            _droneManagerMonitor.DroneNumIncreased += Raise.Event();
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Drones.NewDronesReady);
        }

        [Test]
        public void IdleDrones_Debounces()
        {
            Action debouncedAction = null;
            _debouncer.Debounce(Arg.Do<Action>(x => debouncedAction = x));

            _droneManagerMonitor.IdleDronesStarted += Raise.Event();

            Assert.IsNotNull(debouncedAction);
            debouncedAction.Invoke();
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Drones.Idle);
        }
    }
}
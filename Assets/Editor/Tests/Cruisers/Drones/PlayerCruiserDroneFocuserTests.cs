using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;
using NSubstitute;
using NUnit.Framework;
using System.Diagnostics;
using System;

namespace BattleCruisers.Tests.Cruisers.Drones
{
    public class PlayerCruiserDroneFocuserTests
    {
        private IDroneFocuser _droneFocuser;
        private IDroneManager _droneManager;
        private IDroneFocusSoundPicker _soundPicker;
        private IPrioritisedSoundPlayer _soundPlayer;
        private IDroneConsumer _droneConsumer;
        private PrioritisedSoundKey _soundToPlay;
        private int _playerTriggeredRepairCount;

        [SetUp]
        public void TestSetup()
        {
            _droneManager = Substitute.For<IDroneManager>();
            _soundPicker = Substitute.For<IDroneFocusSoundPicker>();
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();

            _droneFocuser = new PlayerCruiserDroneFocuser(_droneManager, _soundPicker, _soundPlayer);

            _droneConsumer = Substitute.For<IDroneConsumer>();

            _playerTriggeredRepairCount = 0;
            _droneFocuser.PlayerTriggeredRepair += (sender, e) => _playerTriggeredRepairCount++;

            _soundToPlay = PrioritisedSoundKeys.Events.Drones.AllFocused;
            try
            {
                _soundPicker.PickSound(DroneConsumerState.Idle, DroneConsumerState.Focused).Returns(_soundToPlay);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
            }
          
            _droneConsumer.State.Returns(DroneConsumerState.Idle, DroneConsumerState.Focused);
        }

        [Test]
        public void ToggleDroneConsumerFocus_IsTriggeredByPlayer_NotRepair()
        {
            try
            {
                _droneConsumer.NumOfDronesRequired.Returns(RepairManager.NUM_OF_DRONES_REQUIRED_FOR_REPAIR + 1);

                _droneFocuser.ToggleDroneConsumerFocus(_droneConsumer, isTriggeredByPlayer: true);

                _droneManager.Received().ToggleDroneConsumerFocus(_droneConsumer);
                _soundPicker.Received().PickSound(DroneConsumerState.Idle, DroneConsumerState.Focused);
                _soundPlayer.Received().PlaySound(_soundToPlay);
                Assert.AreEqual(0, _playerTriggeredRepairCount);

            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
            }
       
        }

        [Test]
        public void ToggleDroneConsumerFocus_IsTriggeredByPlayer_IsRepair()
        {

            try
            {
                _droneConsumer.NumOfDronesRequired.Returns(RepairManager.NUM_OF_DRONES_REQUIRED_FOR_REPAIR);

                _droneFocuser.ToggleDroneConsumerFocus(_droneConsumer, isTriggeredByPlayer: true);

                _droneManager.Received().ToggleDroneConsumerFocus(_droneConsumer);
                _soundPicker.Received().PickSound(DroneConsumerState.Idle, DroneConsumerState.Focused);
                _soundPlayer.Received().PlaySound(_soundToPlay);
                Assert.AreEqual(1, _playerTriggeredRepairCount);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
            }
        }

        [Test]
        public void ToggleDroneConsumerFocus_IsNotTriggeredByPlayer_DoesNotPlaySound()
        {
            _droneFocuser.ToggleDroneConsumerFocus(_droneConsumer, isTriggeredByPlayer: false);

            _droneManager.Received().ToggleDroneConsumerFocus(_droneConsumer);
            _soundPicker.DidNotReceiveWithAnyArgs().PickSound(default, default);
            _soundPlayer.DidNotReceiveWithAnyArgs().PlaySound(null);
            Assert.AreEqual(0, _playerTriggeredRepairCount);
        }
    }
}
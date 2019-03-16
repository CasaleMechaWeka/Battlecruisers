using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using NUnit.Framework;
using System.Collections.Generic;

namespace BattleCruisers.Tests.Cruisers.Drones
{
    public class PickerTestCase
    {
        public DroneConsumerState PreFocusState { get; }
        public DroneConsumerState PostFocusState { get; }
        public PrioritisedSoundKey ExpectedSound { get; }

        public PickerTestCase(DroneConsumerState preFocusState, DroneConsumerState postFocusState, PrioritisedSoundKey expectedSound)
        {
            PreFocusState = preFocusState;
            PostFocusState = postFocusState;
            ExpectedSound = expectedSound;
        }
    }

    public class DroneFocusSoundPickerTests
    {
        [Test]
        public void PickSound()
        {
            IDroneFocusSoundPicker soundPicker = new DroneFocusSoundPicker();

            IList<PickerTestCase> testCases = new List<PickerTestCase>()
            {
                // To idle
                new PickerTestCase(DroneConsumerState.Idle, DroneConsumerState.Idle, PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToFocus),
                new PickerTestCase(DroneConsumerState.Active, DroneConsumerState.Idle, PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToFocus),
                new PickerTestCase(DroneConsumerState.Focused, DroneConsumerState.Idle, PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToFocus),

                // To active
                new PickerTestCase(DroneConsumerState.Idle, DroneConsumerState.Active, PrioritisedSoundKeys.Events.Drones.Focusing),
                new PickerTestCase(DroneConsumerState.Active, DroneConsumerState.Active, PrioritisedSoundKeys.Events.Drones.AllFocused),
                new PickerTestCase(DroneConsumerState.Focused, DroneConsumerState.Active, PrioritisedSoundKeys.Events.Drones.Dispersing),

                // To focused
                new PickerTestCase(DroneConsumerState.Idle, DroneConsumerState.Focused, PrioritisedSoundKeys.Events.Drones.AllFocused),
                new PickerTestCase(DroneConsumerState.Active, DroneConsumerState.Focused, PrioritisedSoundKeys.Events.Drones.AllFocused),
                new PickerTestCase(DroneConsumerState.Focused, DroneConsumerState.Focused, PrioritisedSoundKeys.Events.Drones.AllFocused)
            };

            foreach (PickerTestCase testCase in testCases)
            {
                PrioritisedSoundKey chosenSound = soundPicker.PickSound(testCase.PreFocusState, testCase.PostFocusState);
                Assert.AreEqual(testCase.ExpectedSound, chosenSound);
            }
        }
    }
}
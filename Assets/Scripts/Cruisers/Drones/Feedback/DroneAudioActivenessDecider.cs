using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    // FELIX  use, test
    public class DroneAudioActivenessDecider : IDroneAudioActivenessDecider
    {
        private readonly IDroneMonitor _droneMonitor;
        private readonly int _maxActiveDroneAudioSources;

        private const int DEFAULT_MAX_ACTIVE_DRONE_AUDIO_SOURCES = 4;

        public DroneAudioActivenessDecider(IDroneMonitor droneMonitor, int maxActiveDroneAudioSources = DEFAULT_MAX_ACTIVE_DRONE_AUDIO_SOURCES)
        {
            Assert.IsNotNull(droneMonitor);

            _droneMonitor = droneMonitor;
            _maxActiveDroneAudioSources = maxActiveDroneAudioSources;
        }

        public bool ShouldHaveAudio(Faction droneFaction)
        {
            Assert.IsTrue(_droneMonitor.FactionToActiveDroneNum.ContainsKey(droneFaction));

            int numOfActiveDrones = _droneMonitor.FactionToActiveDroneNum[droneFaction];
            bool shouldPlaySound = numOfActiveDrones < _maxActiveDroneAudioSources;
            Logging.Log(Tags.DRONE_FEEDBACK, $"Faction: {droneFaction}  Active drone #: {numOfActiveDrones}  shouldMakeSound: {shouldPlaySound}");

            return shouldPlaySound;
        }
    }
}
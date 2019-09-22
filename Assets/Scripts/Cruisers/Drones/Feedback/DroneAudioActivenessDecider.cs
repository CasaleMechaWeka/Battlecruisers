using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneAudioActivenessDecider : IDroneAudioActivenessDecider
    {
        private readonly IReadOnlyDictionary<Faction, int> _factionToActiveDroneNum;
        private readonly int _maxActiveDroneAudioSources;

        private const int DEFAULT_MAX_ACTIVE_DRONE_AUDIO_SOURCES = 4;

        public DroneAudioActivenessDecider(IReadOnlyDictionary<Faction, int> factionToActiveDroneNum, int maxActiveDroneAudioSources = DEFAULT_MAX_ACTIVE_DRONE_AUDIO_SOURCES)
        {
            Assert.IsNotNull(factionToActiveDroneNum);

            _factionToActiveDroneNum = factionToActiveDroneNum;
            _maxActiveDroneAudioSources = maxActiveDroneAudioSources;
        }

        public bool ShouldHaveAudio(Faction droneFaction)
        {
            Assert.IsTrue(_factionToActiveDroneNum.ContainsKey(droneFaction));

            int numOfActiveDrones = _factionToActiveDroneNum[droneFaction];
            bool shouldPlaySound = numOfActiveDrones < _maxActiveDroneAudioSources;
            Logging.Log(Tags.DRONE_FEEDBACK, $"Faction: {droneFaction}  Active drone #: {numOfActiveDrones}  shouldMakeSound: {shouldPlaySound}");

            return shouldPlaySound;
        }
    }
}
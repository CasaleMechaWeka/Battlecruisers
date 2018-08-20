using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class TimeBC : ITime
    {
        public float TimeScale
        {
            get { return Time.timeScale; }
            set { Time.timeScale = value; }
        }

        public float TimeSinceGameStartInS { get { return Time.time; } }
        public float UnscaledDeltaTime { get { return Time.unscaledDeltaTime; } }
    }
}
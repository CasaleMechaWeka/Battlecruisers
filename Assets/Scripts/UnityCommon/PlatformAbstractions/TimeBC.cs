using BattleCruisers.Utils;
using UnityEngine;

namespace UnityCommon.PlatformAbstractions
{
    public class TimeBC : ITime
    {
        private readonly float _defaultFixedDeltaTime;

        public TimeBC()
        {
            _defaultFixedDeltaTime = Time.fixedDeltaTime;
        }

        public float TimeScale
        {
            get { return Time.timeScale; }
            set
            {
                Time.timeScale = value;

                // Also adjust physics time scale
                Time.fixedDeltaTime = _defaultFixedDeltaTime * Time.timeScale;

                Logging.Log(Tags.TIME, $"time scale: {Time.timeScale}  fixed delta time: {Time.fixedDeltaTime}");
            }
        }

        public float TimeSinceGameStartInS => Time.time;
        public float UnscaledDeltaTime => Time.unscaledDeltaTime;
        public float DeltaTime => Time.deltaTime;
    }
}
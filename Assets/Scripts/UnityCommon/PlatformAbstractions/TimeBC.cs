using UnityEngine;

namespace UnityCommon.PlatformAbstractions
{
    public class TimeBC : ITime
    {
        public float TimeScale
        {
            get { return Time.timeScale; }
            set { Time.timeScale = value; }
        }

        public float TimeSinceGameStartInS => Time.time;
        public float UnscaledDeltaTime => Time.unscaledDeltaTime;
        public float DeltaTime => Time.deltaTime;
    }
}
using PlatformTime = UnityEngine.Time;

namespace BattleCruisers.Utils.PlatformAbstractions.Time
{
    public class TimeBC : ITime
    {
        private readonly float _defaultFixedDeltaTime;

        private static ITime _instance;
        public static ITime Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TimeBC();
                }
                return _instance;
            }
        }

        private TimeBC()
        {
            _defaultFixedDeltaTime = PlatformTime.fixedDeltaTime;
            TimeSinceGameStartProvider = new TimeSinceGameStartProvider();
            RealTimeSinceGameStartProvider = new RealTimeSinceGameStartProvider();
        }

        public float TimeScale
        {
            get { return PlatformTime.timeScale; }
            set
            {
                PlatformTime.timeScale = value;

                // Also adjust physics time scale
                PlatformTime.fixedDeltaTime = _defaultFixedDeltaTime * PlatformTime.timeScale;

                Logging.Log(Tags.TIME, $"time scale: {PlatformTime.timeScale}  fixed delta time: {PlatformTime.fixedDeltaTime}");
            }
        }

        public float TimeSinceGameStartInS => PlatformTime.time;
        public float UnscaledTimeSinceGameStartInS => PlatformTime.realtimeSinceStartup;
        public float UnscaledDeltaTime => PlatformTime.unscaledDeltaTime;
        public float DeltaTime => PlatformTime.deltaTime;

        public ITimeSinceGameStartProvider TimeSinceGameStartProvider { get; }
        public ITimeSinceGameStartProvider RealTimeSinceGameStartProvider { get; }
    }
}
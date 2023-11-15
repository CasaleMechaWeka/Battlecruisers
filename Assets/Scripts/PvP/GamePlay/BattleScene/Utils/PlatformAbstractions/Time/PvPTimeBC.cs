using PvPPlatformTime = UnityEngine.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time
{
    public class PvPTimeBC : IPvPTime
    {
        private readonly float _defaultFixedDeltaTime;

        private static IPvPTime _instance;
        public static IPvPTime Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PvPTimeBC();
                }
                return _instance;
            }
        }

        private PvPTimeBC()
        {
            _defaultFixedDeltaTime = PvPPlatformTime.fixedDeltaTime;
            TimeSinceGameStartProvider = new PvPTimeSinceGameStartProvider();
            RealTimeSinceGameStartProvider = new PvPRealTimeSinceGameStartProvider();
        }

        public float TimeScale
        {
            get { return PvPPlatformTime.timeScale; }
            set
            {
                PvPPlatformTime.timeScale = value;

                // Also adjust physics time scale
                PvPPlatformTime.fixedDeltaTime = _defaultFixedDeltaTime * PvPPlatformTime.timeScale;

                // Logging.Log(Tags.TIME, $"time scale: {PvPPlatformTime.timeScale}  fixed delta time: {PvPPlatformTime.fixedDeltaTime}");
            }
        }

        public float TimeSinceGameStartInS => PvPPlatformTime.time;
        public float UnscaledTimeSinceGameStartInS => PvPPlatformTime.realtimeSinceStartup;
        public float UnscaledDeltaTime => PvPPlatformTime.unscaledDeltaTime;
        public float DeltaTime => PvPPlatformTime.deltaTime;

        public IPvPTimeSinceGameStartProvider TimeSinceGameStartProvider { get; }
        public IPvPTimeSinceGameStartProvider RealTimeSinceGameStartProvider { get; }
    }
}


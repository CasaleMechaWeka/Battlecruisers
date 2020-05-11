namespace UnityCommon.PlatformAbstractions.Time
{
    public class TimeSinceGameStartProvider : ITimeSinceGameStartProvider
    {
        public float TimeSinceGameStartInS => TimeBC.Instance.TimeSinceGameStartInS;
    }
}
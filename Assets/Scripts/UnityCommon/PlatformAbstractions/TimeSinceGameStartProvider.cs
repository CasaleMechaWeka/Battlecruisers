namespace UnityCommon.PlatformAbstractions
{
    public class TimeSinceGameStartProvider : ITimeSinceGameStartProvider
    {
        public float TimeSinceGameStartInS => TimeBC.Instance.TimeSinceGameStartInS;
    }
}
namespace UnityCommon.PlatformAbstractions
{
    public class RealTimeSinceGameStartProvider : ITimeSinceGameStartProvider
    {
        public float TimeSinceGameStartInS => TimeBC.Instance.UnscaledTimeSinceGameStartInS;
    }
}
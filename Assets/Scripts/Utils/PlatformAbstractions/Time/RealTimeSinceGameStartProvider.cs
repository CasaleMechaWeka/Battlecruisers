namespace BattleCruisers.Utils.PlatformAbstractions.Time
{
    public class RealTimeSinceGameStartProvider : ITimeSinceGameStartProvider
    {
        public float TimeSinceGameStartInS => TimeBC.Instance.UnscaledTimeSinceGameStartInS;
    }
}
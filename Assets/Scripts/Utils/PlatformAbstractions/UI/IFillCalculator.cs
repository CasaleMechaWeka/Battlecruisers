namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public interface IFillCalculator
    {
        float RawToAdjusted(float rawFillAmount);
        float AdjustedToRaw(float adjustedFillAmount);
    }
}
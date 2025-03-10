namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public interface IFillableImage
    {
        bool IsVisible { get; set; }
        float FillAmount { get; set; }
    }
}
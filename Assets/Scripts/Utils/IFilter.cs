using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Utils
{
    public interface IFilter<TElement>
    {
        bool IsMatch(TElement element);
        bool IsMatch(TElement element, VariantPrefab variant);
    }
}

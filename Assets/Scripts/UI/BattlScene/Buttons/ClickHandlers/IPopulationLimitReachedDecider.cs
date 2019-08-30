using BattleCruisers.Buildables.Buildings.Factories;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    public interface IPopulationLimitReachedDecider
    {
        bool ShouldPlayPopulationLimitReachedWarning(IFactory factory, bool isPopulationLimitReached);
    }
}
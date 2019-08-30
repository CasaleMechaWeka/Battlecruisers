using BattleCruisers.Buildables.Buildings.Factories;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    public class PopulationLimitReachedDecider : IPopulationLimitReachedDecider
    {
        public bool ShouldPlayPopulationLimitReachedWarning(IFactory factory, bool isPopulationLimitReached)
        {
            return
                isPopulationLimitReached
                && factory.UnitUnderConstruction == null
                && !factory.IsUnitPaused.Value;
        }
    }
}
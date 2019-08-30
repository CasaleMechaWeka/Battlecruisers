using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers.Construction;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    // FELIX  Test :)
    public class PopulationLimitReachedDecider : IPopulationLimitReachedDecider
    {
        private readonly IPopulationLimitMonitor _populationLimitMonitor;

        public PopulationLimitReachedDecider(IPopulationLimitMonitor populationLimitMonitor)
        {
            Assert.IsNotNull(populationLimitMonitor);
            _populationLimitMonitor = populationLimitMonitor;
        }

        public bool ShouldPlayPopulationLimitReachedWarning(IFactory factory)
        {
            return
                _populationLimitMonitor.IsPopulationLimitReached
                && factory.UnitUnderConstruction == null
                && !factory.IsUnitPaused.Value;
        }
    }
}
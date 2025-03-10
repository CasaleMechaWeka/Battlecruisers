using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers.Construction;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
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
            Assert.IsNotNull(factory);

            return
                _populationLimitMonitor.IsPopulationLimitReached.Value
                && factory.UnitUnderConstruction == null
                && !factory.IsUnitPaused.Value;
        }
    }
}
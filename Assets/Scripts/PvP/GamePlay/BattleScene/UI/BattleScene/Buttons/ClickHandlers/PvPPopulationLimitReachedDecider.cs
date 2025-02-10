using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers
{
    public class PvPPopulationLimitReachedDecider : IPvPPopulationLimitReachedDecider
    {
        private readonly IPopulationLimitMonitor _populationLimitMonitor;

        public PvPPopulationLimitReachedDecider(IPopulationLimitMonitor populationLimitMonitor)
        {
            Assert.IsNotNull(populationLimitMonitor);
            _populationLimitMonitor = populationLimitMonitor;
        }

        public bool ShouldPlayPopulationLimitReachedWarning(PvPCruiser playerCruiser, IPvPFactory factory)
        {
            Assert.IsNotNull(factory);

            return
                //_populationLimitMonitor.IsPopulationLimitReached.Value
                playerCruiser.pvp_popLimitReachedFeedback.Value
                && factory.UnitUnderConstruction == null
                && !factory.IsUnitPaused.Value;
        }
    }
}
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.BattleScene.InGameHints
{
    public class HintManager
    {
        private readonly IBuildingMonitor _enemyBuildingMonitor;
        private readonly IHintDisplayer _hintDisplayer;

        public HintManager(IBuildingMonitor enemyBuildingMonitor, IHintDisplayer hintDisplayer)
        {
            Helper.AssertIsNotNull(enemyBuildingMonitor, hintDisplayer);

            _enemyBuildingMonitor = enemyBuildingMonitor;
            _hintDisplayer = hintDisplayer;

            _enemyBuildingMonitor.AirFactoryStarted += _buildingMonitor_AirFactoryStarted;
            _enemyBuildingMonitor.NavalFactoryStarted += _buildingMonitor_NavalFactoryStarted;
            _enemyBuildingMonitor.OffensiveStarted += _buildingMonitor_OffensiveStarted;
        }

        private void _buildingMonitor_AirFactoryStarted(object sender, EventArgs e)
        {
            _hintDisplayer.ShowHint(Hints.AIR_FACTORY_RESPONSE_HINT);
        }

        private void _buildingMonitor_NavalFactoryStarted(object sender, EventArgs e)
        {
            _hintDisplayer.ShowHint(Hints.NAVAL_FACTORY_RESPONSE_HINT);
        }

        private void _buildingMonitor_OffensiveStarted(object sender, EventArgs e)
        {
            _hintDisplayer.ShowHint(Hints.OFFENSIVE_RESPONSE_HINT);
        }
    }
}
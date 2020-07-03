using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.BattleScene.InGameHints
{
    public class HintManager
    {
        private readonly IBuildingMonitor _enemyBuildingMonitor;
        private readonly IFactoryMonitor _friendlyFactoryMonitor;
        private readonly IHintDisplayer _hintDisplayer;

        public HintManager(
            IBuildingMonitor enemyBuildingMonitor,
            IFactoryMonitor friendlyFactoryMonitor,
            IHintDisplayer hintDisplayer)
        {
            Helper.AssertIsNotNull(enemyBuildingMonitor, friendlyFactoryMonitor, hintDisplayer);

            _enemyBuildingMonitor = enemyBuildingMonitor;
            _hintDisplayer = hintDisplayer;
            _friendlyFactoryMonitor = friendlyFactoryMonitor;

            _enemyBuildingMonitor.AirFactoryStarted += _buildingMonitor_AirFactoryStarted;
            _enemyBuildingMonitor.NavalFactoryStarted += _buildingMonitor_NavalFactoryStarted;
            _enemyBuildingMonitor.OffensiveStarted += _buildingMonitor_OffensiveStarted;
            _friendlyFactoryMonitor.FactoryCompleted += _friendlyFactoryMonitor_FactoryCompleted;
            _friendlyFactoryMonitor.UnitChosen += _friendlyFactoryMonitor_UnitChosen;
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

        // FELIX  Update tests :)
        private void _friendlyFactoryMonitor_FactoryCompleted(object sender, EventArgs e)
        {
            _hintDisplayer.ShowHint(Hints.FACTORY_COMPLETED_HINT);
        }

        private void _friendlyFactoryMonitor_UnitChosen(object sender, EventArgs e)
        {
            _hintDisplayer.ShowHint(Hints.UNIT_CHOSEN_HINT);
        }
    }
}
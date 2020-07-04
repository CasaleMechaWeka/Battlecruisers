using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.BattleScene.InGameHints
{
    public class HintManager
    {
        private readonly IBuildingMonitor _enemyBuildingMonitor, _friendlyBuildingMonitor;
        private readonly IFactoryMonitor _friendlyFactoryMonitor;
        private readonly IHintDisplayer _hintDisplayer;

        public HintManager(
            IBuildingMonitor enemyBuildingMonitor,
            IBuildingMonitor friendlyBuildingMonitor,
            IFactoryMonitor friendlyFactoryMonitor,
            IHintDisplayer hintDisplayer)
        {
            Helper.AssertIsNotNull(enemyBuildingMonitor, friendlyBuildingMonitor, friendlyFactoryMonitor, hintDisplayer);

            _enemyBuildingMonitor = enemyBuildingMonitor;
            _friendlyBuildingMonitor = friendlyBuildingMonitor;
            _hintDisplayer = hintDisplayer;
            _friendlyFactoryMonitor = friendlyFactoryMonitor;

            _enemyBuildingMonitor.AirFactoryStarted += _buildingMonitor_AirFactoryStarted;
            _friendlyBuildingMonitor.AirDefensiveStarted += _friendlyBuildingMonitor_AirDefensiveStarted;

            _enemyBuildingMonitor.NavalFactoryStarted += _buildingMonitor_NavalFactoryStarted;
            _friendlyBuildingMonitor.ShipDefensiveStarted += _friendlyBuildingMonitor_ShipDefensiveStarted;

            _enemyBuildingMonitor.OffensiveStarted += _buildingMonitor_OffensiveStarted;
            _friendlyBuildingMonitor.ShieldStarted += _friendlyBuildingMonitor_ShieldStarted;
            
            _friendlyFactoryMonitor.FactoryCompleted += _friendlyFactoryMonitor_FactoryCompleted;
            _friendlyFactoryMonitor.UnitChosen += _friendlyFactoryMonitor_UnitChosen;
        }

        private void _buildingMonitor_AirFactoryStarted(object sender, EventArgs e)
        {
            _hintDisplayer.ShowHint(Hints.AIR_FACTORY_RESPONSE_HINT);
        }

        private void _friendlyBuildingMonitor_AirDefensiveStarted(object sender, EventArgs e)
        {
            _hintDisplayer.HideHint(Hints.AIR_FACTORY_RESPONSE_HINT);
        }

        private void _buildingMonitor_NavalFactoryStarted(object sender, EventArgs e)
        {
            _hintDisplayer.ShowHint(Hints.NAVAL_FACTORY_RESPONSE_HINT);
        }
        
        private void _friendlyBuildingMonitor_ShipDefensiveStarted(object sender, EventArgs e)
        {
            _hintDisplayer.HideHint(Hints.NAVAL_FACTORY_RESPONSE_HINT);
        }

        private void _buildingMonitor_OffensiveStarted(object sender, EventArgs e)
        {
            _hintDisplayer.ShowHint(Hints.OFFENSIVE_RESPONSE_HINT);
        }

        private void _friendlyBuildingMonitor_ShieldStarted(object sender, EventArgs e)
        {
            _hintDisplayer.HideHint(Hints.OFFENSIVE_RESPONSE_HINT);
        }

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
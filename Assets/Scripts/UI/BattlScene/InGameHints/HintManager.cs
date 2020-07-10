using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using System;

namespace BattleCruisers.UI.BattleScene.InGameHints
{
    public class HintManager
    {
        private readonly IHintDisplayer _hintDisplayer;

        public HintManager(
            IBuildingMonitor enemyBuildingMonitor,
            IBuildingMonitor friendlyBuildingMonitor,
            IFactoryMonitor friendlyFactoryMonitor,
            IGameEndMonitor gameEndMonitor,
            IHintDisplayer hintDisplayer)
        {
            Helper.AssertIsNotNull(enemyBuildingMonitor, friendlyBuildingMonitor, friendlyFactoryMonitor, gameEndMonitor, hintDisplayer);

            _hintDisplayer = hintDisplayer;

            enemyBuildingMonitor.AirFactoryStarted += buildingMonitor_AirFactoryStarted;
            friendlyBuildingMonitor.AirDefensiveStarted += friendlyBuildingMonitor_AirDefensiveStarted;

            enemyBuildingMonitor.NavalFactoryStarted += buildingMonitor_NavalFactoryStarted;
            friendlyBuildingMonitor.ShipDefensiveStarted += friendlyBuildingMonitor_ShipDefensiveStarted;

            enemyBuildingMonitor.OffensiveStarted += buildingMonitor_OffensiveStarted;
            friendlyBuildingMonitor.ShieldStarted += friendlyBuildingMonitor_ShieldStarted;
            
            friendlyFactoryMonitor.FactoryCompleted += friendlyFactoryMonitor_FactoryCompleted;
            friendlyFactoryMonitor.UnitChosen += friendlyFactoryMonitor_UnitChosen;

            gameEndMonitor.GameEnded += GameEndMonitor_GameEnded;
        }

        private void buildingMonitor_AirFactoryStarted(object sender, EventArgs e)
        {
            _hintDisplayer.ShowHint(Hints.AIR_FACTORY_RESPONSE_HINT);
        }

        private void friendlyBuildingMonitor_AirDefensiveStarted(object sender, EventArgs e)
        {
            _hintDisplayer.HideHint(Hints.AIR_FACTORY_RESPONSE_HINT);
        }

        private void buildingMonitor_NavalFactoryStarted(object sender, EventArgs e)
        {
            _hintDisplayer.ShowHint(Hints.NAVAL_FACTORY_RESPONSE_HINT);
        }
        
        private void friendlyBuildingMonitor_ShipDefensiveStarted(object sender, EventArgs e)
        {
            _hintDisplayer.HideHint(Hints.NAVAL_FACTORY_RESPONSE_HINT);
        }

        private void buildingMonitor_OffensiveStarted(object sender, EventArgs e)
        {
            _hintDisplayer.ShowHint(Hints.OFFENSIVE_RESPONSE_HINT);
        }

        private void friendlyBuildingMonitor_ShieldStarted(object sender, EventArgs e)
        {
            _hintDisplayer.HideHint(Hints.OFFENSIVE_RESPONSE_HINT);
        }

        private void friendlyFactoryMonitor_FactoryCompleted(object sender, EventArgs e)
        {
            _hintDisplayer.ShowHint(Hints.FACTORY_COMPLETED_HINT);
        }

        private void friendlyFactoryMonitor_UnitChosen(object sender, EventArgs e)
        {
            _hintDisplayer.ShowHint(Hints.UNIT_CHOSEN_HINT);
        }

        private void GameEndMonitor_GameEnded(object sender, EventArgs e)
        {
            _hintDisplayer.HideAllHints();
        }
    }
}
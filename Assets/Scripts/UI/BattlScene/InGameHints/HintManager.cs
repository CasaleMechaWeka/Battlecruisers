using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.BattleScene.InGameHints
{
    // FELIX  Use, test
    public class HintManager
    {
        private readonly IBuildingMonitor _buildingMonitor;
        private readonly IHintDisplayer _hintDisplayer;

        public HintManager(IBuildingMonitor buildingMonitor, IHintDisplayer hintDisplayer)
        {
            Helper.AssertIsNotNull(buildingMonitor, hintDisplayer);

            _buildingMonitor = buildingMonitor;
            _hintDisplayer = hintDisplayer;

            _buildingMonitor.AirFactoryStarted += _buildingMonitor_AirFactoryStarted;
            _buildingMonitor.NavalFactoryStarted += _buildingMonitor_NavalFactoryStarted;
            _buildingMonitor.OffensiveStarted += _buildingMonitor_OffensiveStarted;
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
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public class PvPPopulationLimitMonitor : IPvPPopulationLimitMonitor
    {
        private readonly IPvPCruiserUnitMonitor _unitMonitor;

        private readonly IPvPSettableBroadcastingProperty<bool> _isPopulationLimitReached;
        public IPvPBroadcastingProperty<bool> IsPopulationLimitReached { get; }

        public PvPPopulationLimitMonitor(IPvPCruiserUnitMonitor unitMonitor)
        {
            Assert.IsNotNull(unitMonitor);

            _isPopulationLimitReached = new PvPSettableBroadcastingProperty<bool>(initialValue: false);
            IsPopulationLimitReached = new PvPBroadcastingProperty<bool>(_isPopulationLimitReached);

            _unitMonitor = unitMonitor;
            _unitMonitor.UnitCompleted += _unitMonitor_UnitCompleted;
            _unitMonitor.UnitDestroyed += _unitMonitor_UnitDestroyed;
        }

        private void _unitMonitor_UnitDestroyed(object sender, PvPUnitDestroyedEventArgs e)
        {
            CheckIfPopulationReached();
        }

        private void _unitMonitor_UnitCompleted(object sender, PvPUnitCompletedEventArgs e)
        {
            CheckIfPopulationReached();
        }

        private void CheckIfPopulationReached()
        {
            _isPopulationLimitReached.Value = _unitMonitor.AliveUnits.Count >= PvPConstants.POPULATION_LIMIT;
        }
    }
}
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Construction
{
    public class PopulationLimitMonitor : IPopulationLimitMonitor
    {
        private readonly ICruiserUnitMonitor _unitMonitor;

        private readonly ISettableBroadcastingProperty<bool> _isPopulationLimitReached;
        public IBroadcastingProperty<bool> IsPopulationLimitReached { get; }

        public PopulationLimitMonitor(ICruiserUnitMonitor unitMonitor)
        {
            Assert.IsNotNull(unitMonitor);

            _isPopulationLimitReached = new SettableBroadcastingProperty<bool>(initialValue: false);
            IsPopulationLimitReached = new BroadcastingProperty<bool>(_isPopulationLimitReached);

            _unitMonitor = unitMonitor;
            _unitMonitor.UnitCompleted += _unitMonitor_UnitCompleted;
            _unitMonitor.UnitDestroyed += _unitMonitor_UnitDestroyed;
        }

        private void _unitMonitor_UnitDestroyed(object sender, UnitDestroyedEventArgs e)
        {
            CheckIfPopulationReached();
        }

        private void _unitMonitor_UnitCompleted(object sender, UnitCompletedEventArgs e)
        {
            CheckIfPopulationReached();
        }

        private void CheckIfPopulationReached()
        {
            _isPopulationLimitReached.Value = _unitMonitor.AliveUnits.Count >= Constants.POPULATION_LIMIT;
        }
    }
}
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Construction
{
    public class PopulationLimitMonitor : IPopulationLimitMonitor
    {
        private readonly ICruiserUnitMonitor _unitMonitor;

        public bool IsPopulationLimitReached => _unitMonitor.AliveUnits.Count >= Constants.POPULATION_LIMIT;

        public event EventHandler PopulationLimitReached;

        public PopulationLimitMonitor(ICruiserUnitMonitor unitMonitor)
        {
            Assert.IsNotNull(unitMonitor);

            _unitMonitor = unitMonitor;
            _unitMonitor.UnitCompleted += _unitMonitor_UnitCompleted;
        }

        private void _unitMonitor_UnitCompleted(object sender, UnitCompletedEventArgs e)
        {
            if (IsPopulationLimitReached)
            {
                PopulationLimitReached?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
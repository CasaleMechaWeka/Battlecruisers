using System;
using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.Filters
{
    public class AffordableBuildableFilter : IFilter<IBuildable>
    {
        private readonly IDroneManager _droneManager;

		public event EventHandler PotentialMatchChange;

        public AffordableBuildableFilter(IDroneManager droneManager)
        {
            Assert.IsNotNull(droneManager);

            _droneManager = droneManager;
            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
        }

        private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            if (PotentialMatchChange != null)
            {
                PotentialMatchChange.Invoke(this, EventArgs.Empty);
            }
        }

        /// <returns><c>true</c>, if the buildable is affordable (can be built), <c>false</c> otherwise.</returns>
        public bool IsMatch(IBuildable buildable)
        {
            return _droneManager.CanSupportDroneConsumer(buildable.NumOfDronesRequired);
        }
    }
}

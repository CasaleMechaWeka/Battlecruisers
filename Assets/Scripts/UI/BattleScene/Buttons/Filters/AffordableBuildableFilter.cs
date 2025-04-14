using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.Filters
{
    public class AffordableBuildableFilter : IBroadcastingFilter<IBuildable>
    {
        private readonly DroneManager _droneManager;

        public event EventHandler PotentialMatchChange;

        public AffordableBuildableFilter(DroneManager droneManager)
        {
            Assert.IsNotNull(droneManager);

            _droneManager = droneManager;
            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
        }

        private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            PotentialMatchChange?.Invoke(this, EventArgs.Empty);
        }

        /// <returns><c>true</c>, if the buildable is affordable (can be built), <c>false</c> otherwise.</returns>
        public bool IsMatch(IBuildable buildable)
        {
            return _droneManager.CanSupportDroneConsumer(buildable.NumOfDronesRequired);
        }
        public bool IsMatch(IBuildable buildable, VariantPrefab variant)
        {
            if (variant != null)
                return _droneManager.CanSupportDroneConsumer(buildable.NumOfDronesRequired + variant.statVariant.drone_num);
            return _droneManager.CanSupportDroneConsumer(buildable.NumOfDronesRequired);
        }
    }
}

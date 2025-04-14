using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters
{
    public class PvPAffordableBuildableFilter : IBroadcastingFilter<IPvPBuildable>
    {
        private readonly DroneManager _droneManager;

        public event EventHandler PotentialMatchChange;
        private readonly PvPCruiser _playerCruiser;


        // we don't use this constructor :D
        public PvPAffordableBuildableFilter(DroneManager droneManager)
        {
            Assert.IsNotNull(droneManager);

            _droneManager = droneManager;
            //     _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
        }


        public PvPAffordableBuildableFilter(PvPCruiser playerCruiser)
        {
            Assert.IsNotNull(playerCruiser);

            // _droneManager = droneManager;
            // _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            _playerCruiser = playerCruiser;
            _playerCruiser.pvp_NumOfDrones.OnValueChanged += _droneManager_DroneNumChanged;

        }

        private void _droneManager_DroneNumChanged(int oldVal, int newVal /*object sender, DroneNumChangedEventArgs e*/)
        {
            PotentialMatchChange?.Invoke(this, EventArgs.Empty);
        }

        /// <returns><c>true</c>, if the buildable is affordable (can be built), <c>false</c> otherwise.</returns>
        public bool IsMatch(IPvPBuildable buildable)
        {
            //  return true;
            //  return _droneManager.CanSupportDroneConsumer(buildable.NumOfDronesRequired);
            // NumOfDrones >= numOfDronesRequired;
            return _playerCruiser.pvp_NumOfDrones.Value >= buildable.NumOfDronesRequired;
        }


        public bool IsMatch(IPvPBuildable buildable, VariantPrefab variant)
        {
            if (variant != null)
                return _playerCruiser.pvp_NumOfDrones.Value >= (buildable.NumOfDronesRequired + variant.statVariant.drone_num);
            return _playerCruiser.pvp_NumOfDrones.Value >= buildable.NumOfDronesRequired;
        }
    }
}

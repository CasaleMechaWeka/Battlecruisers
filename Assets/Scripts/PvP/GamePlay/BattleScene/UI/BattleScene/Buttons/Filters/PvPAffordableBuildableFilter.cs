using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters
{
    public class PvPAffordableBuildableFilter : IPvPBroadcastingFilter<IPvPBuildable>
    {
        private readonly IPvPDroneManager _droneManager;

        public event EventHandler PotentialMatchChange;
        private readonly PvPCruiser _playerCruiser;

        public PvPAffordableBuildableFilter(IPvPDroneManager droneManager)
        {
            Assert.IsNotNull(droneManager);

            _droneManager = droneManager;
            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
        }


        public PvPAffordableBuildableFilter(PvPCruiser playerCruiser)
        {
             Assert.IsNotNull(playerCruiser);

            // _droneManager = droneManager;
            // _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            _playerCruiser = playerCruiser;
        }

        private void _droneManager_DroneNumChanged(object sender, PvPDroneNumChangedEventArgs e)
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
    }
}

using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Drones;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.Click
{
    // FELIX  Test :)
    public class PlayerBuildingDoubleClickHandler : IDoubleClickHandler<IBuilding>
    {
        private readonly IDroneManager _playerDroneManager;

        public PlayerBuildingDoubleClickHandler(IDroneManager playerDroneManager)
        {
            Assert.IsNotNull(playerDroneManager);
            _playerDroneManager = playerDroneManager;
        }

        public void OnDoubleClick(IBuilding playerBuliding)
        {
            Assert.AreEqual(Faction.Blues, playerBuliding.Faction);

            // Toggle drone consumer focus on double click :)
            if (playerBuliding.BuildableState == BuildableState.NotStarted
                || playerBuliding.BuildableState == BuildableState.InProgress
                || playerBuliding.BuildableState == BuildableState.Paused)
            {
                _playerDroneManager.ToggleDroneConsumerFocus(playerBuliding.DroneConsumer);
            }
            // Toggle repair drone consumer focus
            else if (playerBuliding.BuildableState == BuildableState.Completed
                && playerBuliding.RepairCommand.CanExecute)
            {
                IDroneConsumer repairDroneConsumer = playerBuliding.ParentCruiser.RepairManager.GetDroneConsumer(playerBuliding);
                _playerDroneManager.ToggleDroneConsumerFocus(repairDroneConsumer);
            }
        }
    }
}
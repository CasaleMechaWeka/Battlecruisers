using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Drones;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.Click
{
    // FELIX  Test :)
    public class PlayerBuildingDoubleClickHandler : IDoubleClickHandler<IBuilding>
    {
        public void OnDoubleClick(IBuilding playerBuliding)
        {
            Assert.AreEqual(Faction.Blues, playerBuliding.Faction);

            // Toggle drone consumer focus on double click :)
            if (playerBuliding.BuildableState == BuildableState.NotStarted
                || playerBuliding.BuildableState == BuildableState.InProgress
                || playerBuliding.BuildableState == BuildableState.Paused)
            {
                playerBuliding.ParentCruiser.DroneManager.ToggleDroneConsumerFocus(playerBuliding.DroneConsumer);
            }
            // Toggle repair drone consumer focus
            else if (playerBuliding.BuildableState == BuildableState.Completed
                && playerBuliding.RepairCommand.CanExecute)
            {
                IDroneConsumer repairDroneConsumer = playerBuliding.ParentCruiser.RepairManager.GetDroneConsumer(playerBuliding);
                playerBuliding.ParentCruiser.DroneManager.ToggleDroneConsumerFocus(repairDroneConsumer);
            }
        }
    }
}
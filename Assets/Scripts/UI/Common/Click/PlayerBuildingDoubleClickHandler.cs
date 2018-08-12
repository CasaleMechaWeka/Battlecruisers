using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Drones;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.Click
{
    public class PlayerBuildingDoubleClickHandler : IDoubleClickHandler<IBuilding>
    {
        public void OnDoubleClick(IBuilding playerBuliding)
        {
            Assert.AreEqual(Faction.Blues, playerBuliding.Faction);

            if (playerBuliding.BuildableState != BuildableState.Completed)
            {
                // Toggle construction drone consumer focus
                playerBuliding.ParentCruiser.DroneManager.ToggleDroneConsumerFocus(playerBuliding.DroneConsumer);
            }
            else if (playerBuliding.RepairCommand.CanExecute)
            {
                // Toggle repair drone consumer focus
                IDroneConsumer repairDroneConsumer = playerBuliding.ParentCruiser.RepairManager.GetDroneConsumer(playerBuliding);
                playerBuliding.ParentCruiser.DroneManager.ToggleDroneConsumerFocus(repairDroneConsumer);
            }
        }
    }
}
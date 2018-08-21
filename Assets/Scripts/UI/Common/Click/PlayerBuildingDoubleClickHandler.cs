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

            if (playerBuliding.ToggleDroneConsumerFocusCommand.CanExecute)
            {
                // Building construction or factory production
                playerBuliding.ToggleDroneConsumerFocusCommand.Execute();
            }
            else if (playerBuliding.RepairCommand.CanExecute)
            {
                // Building repairs
                IDroneConsumer repairDroneConsumer = playerBuliding.ParentCruiser.RepairManager.GetDroneConsumer(playerBuliding);
                playerBuliding.ParentCruiser.DroneManager.ToggleDroneConsumerFocus(repairDroneConsumer);
            }
        }
    }
}
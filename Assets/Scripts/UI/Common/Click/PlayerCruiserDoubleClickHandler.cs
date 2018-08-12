using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.Click
{
    // FELIX  Test :)
    public class PlayerCruiserDoubleClickHandler : IDoubleClickHandler<ICruiser>
    {
        public void OnDoubleClick(ICruiser playerCruiser)
        {
            Assert.AreEqual(Faction.Blues, playerCruiser.Faction);

            // Toggle repair drone consumer focus
            if (playerCruiser.RepairCommand.CanExecute)
            {
                IDroneConsumer repairDroneConsumer = playerCruiser.RepairManager.GetDroneConsumer(playerCruiser);
                playerCruiser.DroneManager.ToggleDroneConsumerFocus(repairDroneConsumer);
            }
        }
    }
}
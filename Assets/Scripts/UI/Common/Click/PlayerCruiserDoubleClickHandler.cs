using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.Click
{
    public class PlayerCruiserDoubleClickHandler : IDoubleClickHandler<ICruiser>
    {
        public void OnDoubleClick(ICruiser playerCruiser)
        {
            Assert.AreEqual(Faction.Blues, playerCruiser.Faction);

            // Toggle repair drone consumer focus
            if (playerCruiser.RepairCommand.CanExecute)
            {
                IDroneConsumer repairDroneConsumer = playerCruiser.RepairManager.GetDroneConsumer(playerCruiser);
                playerCruiser.DroneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);
            }
        }
    }
}
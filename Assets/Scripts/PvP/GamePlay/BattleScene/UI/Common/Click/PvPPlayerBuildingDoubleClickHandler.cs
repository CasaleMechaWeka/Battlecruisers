using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click
{
    public class PvPPlayerBuildingDoubleClickHandler : IPvPDoubleClickHandler<IPvPBuilding>
    {
        public void OnDoubleClick(IPvPBuilding playerBuliding)
        {
            Assert.AreEqual(PvPFaction.Blues, playerBuliding.Faction);

            if (playerBuliding.ToggleDroneConsumerFocusCommand.CanExecute)
            {
                // Building construction or factory production
                playerBuliding.ToggleDroneConsumerFocusCommand.ExecuteIfPossible();
            }
            else if (playerBuliding.RepairCommand.CanExecute)
            {
                // Building repairs
                IPvPDroneConsumer repairDroneConsumer = playerBuliding.ParentCruiser.RepairManager.GetDroneConsumer(playerBuliding);
                playerBuliding.ParentCruiser.DroneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);
            }
        }
    }
}
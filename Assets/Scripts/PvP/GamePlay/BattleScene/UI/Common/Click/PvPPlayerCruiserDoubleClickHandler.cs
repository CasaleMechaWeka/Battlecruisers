using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click
{
    public class PvPPlayerCruiserDoubleClickHandler : IPvPDoubleClickHandler<IPvPCruiser>
    {
        public void OnDoubleClick(IPvPCruiser playerCruiser)
        {
            Assert.AreEqual(PvPFaction.Blues, playerCruiser.Faction);

            // Toggle repair drone consumer focus
            if (playerCruiser.RepairCommand.CanExecute)
            {
                IPvPDroneConsumer repairDroneConsumer = playerCruiser.RepairManager.GetDroneConsumer(playerCruiser);
                playerCruiser.DroneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);
            }
        }
    }
}
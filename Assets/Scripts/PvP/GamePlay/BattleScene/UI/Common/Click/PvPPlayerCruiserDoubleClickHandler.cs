using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click
{
    public class PvPPlayerCruiserDoubleClickHandler : IPvPDoubleClickHandler<IPvPCruiser>
    {
        public void OnDoubleClick(IPvPCruiser playerCruiser)
        {
     /*       Assert.AreEqual(PvPFaction.Blues, playerCruiser.Faction);*/

            // Toggle repair drone consumer focus

            if(SynchedServerData.Instance.GetTeam() == Team.LEFT)
            {
                if(playerCruiser.Faction == PvPFaction.Blues)
                {
                    if (playerCruiser.RepairCommand.CanExecute)
                    {
                        IPvPDroneConsumer repairDroneConsumer = playerCruiser.RepairManager.GetDroneConsumer(playerCruiser);
                        playerCruiser.DroneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);
                    }
                }
                else
                {
                    PvPBattleSceneGodClient.Instance.userChosenTargetHelper.ToggleChosenTarget(playerCruiser);
                }
            }
            else
            {
                if (playerCruiser.Faction == PvPFaction.Blues)
                {
                    PvPBattleSceneGodClient.Instance.userChosenTargetHelper.ToggleChosenTarget(playerCruiser);
                }
                else
                {
                    if (playerCruiser.RepairCommand.CanExecute)
                    {
                        IPvPDroneConsumer repairDroneConsumer = playerCruiser.RepairManager.GetDroneConsumer(playerCruiser);
                        playerCruiser.DroneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);
                    }
                }
            }
        }
    }
}
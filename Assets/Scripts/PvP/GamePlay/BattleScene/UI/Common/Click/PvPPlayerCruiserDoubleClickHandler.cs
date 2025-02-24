using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click
{
    public class PvPPlayerCruiserDoubleClickHandler : IPvPDoubleClickHandler<IPvPCruiser>
    {
        public void OnDoubleClick(IPvPCruiser playerCruiser)
        {
            /*       Assert.AreEqual(Faction.Blues, playerCruiser.Faction);*/

            // Toggle repair drone consumer focus

            if (playerCruiser.IsAIBot())
            {
                PvPBattleSceneGodClient.Instance.userChosenTargetHelper.ToggleChosenTarget(playerCruiser);
            }
            else
            {
                if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
                {
                    if (playerCruiser.Faction == Faction.Blues)
                    {
                        if (playerCruiser.RepairCommand.CanExecute)
                        {
                            IDroneConsumer repairDroneConsumer = playerCruiser.RepairManager.GetDroneConsumer(playerCruiser);
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
                    if (playerCruiser.Faction == Faction.Blues)
                    {
                        PvPBattleSceneGodClient.Instance.userChosenTargetHelper.ToggleChosenTarget(playerCruiser);
                    }
                    else
                    {
                        if (playerCruiser.RepairCommand.CanExecute)
                        {
                            playerCruiser.clickedRepairButton?.Invoke();
                            /*                        IDroneConsumer repairDroneConsumer = playerCruiser.RepairManager.GetDroneConsumer(playerCruiser);
                                                    playerCruiser.DroneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);*/

                        }
                    }
                }
            }
        }
    }
}
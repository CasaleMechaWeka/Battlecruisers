using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click
{
    public class PvPPlayerBuildingDoubleClickHandler : IPvPDoubleClickHandler<IPvPBuilding>
    {
        public void OnDoubleClick(IPvPBuilding playerBuliding)
        {
            /*   Assert.AreEqual(PvPFaction.Blues, playerBuliding.Faction);*/

            if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
            {
                if (playerBuliding.Faction == PvPFaction.Blues)
                {
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
                else
                {
                    PvPBattleSceneGodClient.Instance.userChosenTargetHelper.ToggleChosenTarget(playerBuliding);
                }
            }
            else
            {
                if (playerBuliding.Faction == PvPFaction.Blues)
                {
                    PvPBattleSceneGodClient.Instance.userChosenTargetHelper.ToggleChosenTarget(playerBuliding);
                }
                else
                {
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
    }
}
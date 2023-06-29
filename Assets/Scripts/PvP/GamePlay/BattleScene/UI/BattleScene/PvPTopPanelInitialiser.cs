using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public class PvPTopPanelInitialiser : MonoBehaviour
    {
        public Text playerLeftHealthBarHelpLabel;
        public Text playerRightHealthBarHelpLabel;

        public PvPTopPanelComponents Initialise(
            PvPCruiser playerCruiser,
            PvPCruiser enemyCruiser,
            string playerName,
            string enemyName)
        {
            PvPHelper.AssertIsNotNull(playerLeftHealthBarHelpLabel, playerRightHealthBarHelpLabel);
            PvPHelper.AssertIsNotNull(playerName, enemyName);
            PvPHelper.AssertIsNotNull(playerCruiser, enemyCruiser);

            IPvPHighlightable playerLeftCruiserHealthBar;
            IPvPHighlightable playerRightCruiserHealthBar;
            if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
            {
                playerLeftHealthBarHelpLabel.text = playerName;
                playerRightHealthBarHelpLabel.text = enemyName;

                PvPCruiserHealthBarInitialiser playerLeftHealthInitialiser = transform.FindNamedComponent<PvPCruiserHealthBarInitialiser>("PlayerLeftCruiserHealth/Foreground");
                Assert.IsNotNull(playerLeftHealthInitialiser);
                playerLeftCruiserHealthBar = playerLeftHealthInitialiser.Initialise(playerCruiser);

                PvPCruiserHealthBarInitialiser playerRightHealthInitialiser = transform.FindNamedComponent<PvPCruiserHealthBarInitialiser>("PlayerRightCruiserHealth/Foreground");
                Assert.IsNotNull(playerRightHealthInitialiser);
                playerRightCruiserHealthBar = playerRightHealthInitialiser.Initialise(enemyCruiser);
            }
            else
            {
                playerLeftHealthBarHelpLabel.text = enemyName;
                playerRightHealthBarHelpLabel.text = playerName;

                PvPCruiserHealthBarInitialiser playerLeftHealthInitialiser = transform.FindNamedComponent<PvPCruiserHealthBarInitialiser>("PlayerLeftCruiserHealth/Foreground");
                Assert.IsNotNull(playerLeftHealthInitialiser);
                playerLeftCruiserHealthBar = playerLeftHealthInitialiser.Initialise(enemyCruiser);

                PvPCruiserHealthBarInitialiser playerRightHealthInitialiser = transform.FindNamedComponent<PvPCruiserHealthBarInitialiser>("PlayerRightCruiserHealth/Foreground");
                Assert.IsNotNull(playerRightHealthInitialiser);
                playerRightCruiserHealthBar = playerRightHealthInitialiser.Initialise(playerCruiser);
            }


            return new PvPTopPanelComponents(playerLeftCruiserHealthBar, playerRightCruiserHealthBar);
        }
    }
}
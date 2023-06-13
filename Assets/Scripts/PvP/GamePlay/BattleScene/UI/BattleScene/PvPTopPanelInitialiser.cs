using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public class PvPTopPanelInitialiser : MonoBehaviour
    {
        public Text playerHealthBarHelpLabel;
        public Text enemyHealthBarHelpLabel;

        public PvPTopPanelComponents Initialise(
            PvPCruiser playerCruiser,
            PvPCruiser enemyCruiser,
            string playerName,
            string enemyName)
        {
            PvPHelper.AssertIsNotNull(playerHealthBarHelpLabel, enemyHealthBarHelpLabel);
            PvPHelper.AssertIsNotNull(playerName, enemyName);
            PvPHelper.AssertIsNotNull(playerCruiser, enemyCruiser);

            PvPCruiserHealthBarInitialiser playerHealthInitialiser = transform.FindNamedComponent<PvPCruiserHealthBarInitialiser>("PlayerCruiserHealth/Foreground");
            Assert.IsNotNull(playerHealthInitialiser);
            IPvPHighlightable playerCruiserHealthBar = playerHealthInitialiser.Initialise(playerCruiser);

            PvPCruiserHealthBarInitialiser enemyHealthInitialiser = transform.FindNamedComponent<PvPCruiserHealthBarInitialiser>("EnemyCruiserHealth/Foreground");
            Assert.IsNotNull(enemyHealthInitialiser);
            IPvPHighlightable enemyCruiserHealthBar = enemyHealthInitialiser.Initialise(enemyCruiser);

            playerHealthBarHelpLabel.text = playerName;
            enemyHealthBarHelpLabel.text = enemyName;

            return new PvPTopPanelComponents(playerCruiserHealthBar, enemyCruiserHealthBar);
        }
    }
}
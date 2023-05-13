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
        public Text enemyHealthBarHelpLabel;

        public PvPTopPanelComponents Initialise(
            IPvPCruiser playerCruiser,
            IPvPCruiser aiCruiser,
            string enemyName)
        {
            Assert.IsNotNull(enemyHealthBarHelpLabel);
            PvPHelper.AssertIsNotNull(playerCruiser, aiCruiser);

            PvPCruiserHealthBarInitialiser playerHealthInitialiser = transform.FindNamedComponent<PvPCruiserHealthBarInitialiser>("PlayerCruiserHealth/Foreground");
            Assert.IsNotNull(playerHealthInitialiser);
            IPvPHighlightable playerCruiserHealthBar = playerHealthInitialiser.Initialise(playerCruiser);

            PvPCruiserHealthBarInitialiser aiHealthInitialiser = transform.FindNamedComponent<PvPCruiserHealthBarInitialiser>("AICruiserHealth/Foreground");
            Assert.IsNotNull(aiHealthInitialiser);
            IPvPHighlightable aiCruiserHealthBar = aiHealthInitialiser.Initialise(aiCruiser);

            enemyHealthBarHelpLabel.text = enemyName;

            return new PvPTopPanelComponents(playerCruiserHealthBar, aiCruiserHealthBar);
        }
    }
}
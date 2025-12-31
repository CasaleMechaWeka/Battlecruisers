using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene
{
    public class TopPanelInitialiser : MonoBehaviour
    {
        public Text enemyHealthBarHelpLabel;

        public TopPanelComponents Initialise(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            string enemyName)
        {
            Assert.IsNotNull(enemyHealthBarHelpLabel);
            Helper.AssertIsNotNull(playerCruiser, aiCruiser);

            CruiserHealthBarInitialiser playerHealthInitialiser = transform.FindNamedComponent<CruiserHealthBarInitialiser>("PlayerCruiserHealth/Foreground");
            Assert.IsNotNull(playerHealthInitialiser);
            IHighlightable playerCruiserHealthBar = playerHealthInitialiser.Initialise(playerCruiser);

            CruiserHealthBarInitialiser aiHealthInitialiser = transform.FindNamedComponent<CruiserHealthBarInitialiser>("AICruiserHealth/Foreground");
            Assert.IsNotNull(aiHealthInitialiser);
            IHighlightable aiCruiserHealthBar = aiHealthInitialiser.Initialise(aiCruiser);

            enemyHealthBarHelpLabel.text = enemyName;

            return new TopPanelComponents(playerCruiserHealthBar, aiCruiserHealthBar);
        }
    }
}
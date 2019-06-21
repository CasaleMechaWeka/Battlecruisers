using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class TopPanelInitialiser : MonoBehaviour
    {
        public TopPanelComponents Initialise(ICruiser playerCruiser, ICruiser aiCruiser)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser);

            PlayerCruiserHealthDialInitialiser playerHealthInitialiser = transform.FindNamedComponent<PlayerCruiserHealthDialInitialiser>("PlayerCruiserHealth/Foreground");
            Assert.IsNotNull(playerHealthInitialiser);
            IMaskHighlightable playerCruiserHealthBar = playerHealthInitialiser.Initialise(playerCruiser);

            PlayerCruiserHealthDialInitialiser aiHealthInitialiser = transform.FindNamedComponent<PlayerCruiserHealthDialInitialiser>("AICruiserHealth/Foreground");
            Assert.IsNotNull(aiHealthInitialiser);
            IMaskHighlightable aiCruiserHealthBar = aiHealthInitialiser.Initialise(aiCruiser);

            return new TopPanelComponents(playerCruiserHealthBar, aiCruiserHealthBar);
        }
    }
}
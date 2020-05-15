using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class TopPanelInitialiser : MonoBehaviour
    {
        public TopPanelComponents Initialise(ICruiser playerCruiser, ICruiser aiCruiser, IBroadcastingFilter helpLabelVisibilityFilter)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, helpLabelVisibilityFilter);

            CruiserHealthBarInitialiser playerHealthInitialiser = transform.FindNamedComponent<CruiserHealthBarInitialiser>("PlayerCruiserHealth/Foreground");
            Assert.IsNotNull(playerHealthInitialiser);
            IHighlightable playerCruiserHealthBar = playerHealthInitialiser.Initialise(playerCruiser, helpLabelVisibilityFilter);

            CruiserHealthBarInitialiser aiHealthInitialiser = transform.FindNamedComponent<CruiserHealthBarInitialiser>("AICruiserHealth/Foreground");
            Assert.IsNotNull(aiHealthInitialiser);
            IHighlightable aiCruiserHealthBar = aiHealthInitialiser.Initialise(aiCruiser, helpLabelVisibilityFilter);

            return new TopPanelComponents(playerCruiserHealthBar, aiCruiserHealthBar);
        }
    }
}
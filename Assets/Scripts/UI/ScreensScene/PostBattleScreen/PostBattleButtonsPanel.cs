using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostBattleButtonsPanel : MonoBehaviour
    {
        public void Initialise(IPostBattleScreen postBattleScreen, ICommand nextCommand)
        {
            Helper.AssertIsNotNull(postBattleScreen, nextCommand);

            ActionButton retryButton = transform.FindNamedComponent<ActionButton>("SmallButtons/RetryButton");
            retryButton.Initialise(postBattleScreen.Retry);

            ActionButton homeButton = transform.FindNamedComponent<ActionButton>("SmallButtons/HomeButton");
            homeButton.Initialise(postBattleScreen.GoToHomeScreen);

            ActionButton loadoutButton = transform.FindNamedComponent<ActionButton>("SmallButtons/LoadoutButton");
            loadoutButton.Initialise(postBattleScreen.GoToLoadoutScreen);

            ButtonController nextButton = GetComponentInChildren<ButtonController>();
            Assert.IsNotNull(nextButton);
            nextButton.Initialise(nextCommand);
        }
    }
}
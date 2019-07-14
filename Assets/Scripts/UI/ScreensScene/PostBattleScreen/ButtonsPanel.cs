using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class ButtonsPanel : MonoBehaviour
    {
        public virtual void Initialise(IPostBattleScreen postBattleScreen)
        {
            Assert.IsNotNull(postBattleScreen);

            ActionButton retryButton = transform.FindNamedComponent<ActionButton>("SmallButtons/RetryButton");
            retryButton.Initialise(postBattleScreen.Retry);

            ActionButton homeButton = transform.FindNamedComponent<ActionButton>("SmallButtons/HomeButton");
            homeButton.Initialise(postBattleScreen.GoToHomeScreen);
        }
    }
}
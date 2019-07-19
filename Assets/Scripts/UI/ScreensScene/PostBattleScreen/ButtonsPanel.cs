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

            ActionButton homeButton = transform.FindNamedComponent<ActionButton>("SmallButtons/HomeButton");
            homeButton.Initialise(postBattleScreen.GoToHomeScreen);
        }
    }
}
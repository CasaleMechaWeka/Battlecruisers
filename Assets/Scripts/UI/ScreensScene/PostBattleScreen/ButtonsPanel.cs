using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class ButtonsPanel : MonoBehaviour
    {
        public ActionButton homeButton;

        public virtual void Initialise(IPostBattleScreen postBattleScreen, ISingleSoundPlayer soundPlayer)
        {
            Assert.IsNotNull(homeButton);
            Helper.AssertIsNotNull(postBattleScreen, soundPlayer);

            homeButton.Initialise(soundPlayer, postBattleScreen.GoToHomeScreen);
        }
    }
}
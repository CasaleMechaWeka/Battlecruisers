using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class ButtonsPanel : MonoBehaviour
    {
        public CanvasGroupButton homeButton;

        public virtual void Initialise(IPostBattleScreen postBattleScreen, ISingleSoundPlayer soundPlayer)
        {
            Assert.IsNotNull(homeButton);
            Helper.AssertIsNotNull(postBattleScreen, soundPlayer);

            homeButton.Initialise(soundPlayer, postBattleScreen.GoToHomeScreen);

            gameObject.SetActive(true);
        }
    }
}
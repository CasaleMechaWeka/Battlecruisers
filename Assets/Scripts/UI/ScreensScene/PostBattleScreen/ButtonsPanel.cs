using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class ButtonsPanel : MonoBehaviour
    {
        public CanvasGroupButton homeButton;

        public virtual void Initialise(PostBattleScreenController postBattleScreen, SingleSoundPlayer soundPlayer)
        {
            Assert.IsNotNull(homeButton);
            Helper.AssertIsNotNull(postBattleScreen, soundPlayer);

            homeButton.Initialise(soundPlayer, postBattleScreen.GoToHomeScreen);
        }
    }
}
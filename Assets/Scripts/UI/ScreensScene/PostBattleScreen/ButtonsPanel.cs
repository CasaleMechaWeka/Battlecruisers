using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class ButtonsPanel : MonoBehaviour
    {
        public virtual void Initialise(IPostBattleScreen postBattleScreen, ISingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(postBattleScreen, soundPlayer);

            ActionButton homeButton = transform.FindNamedComponent<ActionButton>("SmallButtons/HomeButton");
            homeButton.Initialise(soundPlayer, postBattleScreen.GoToHomeScreen);
        }
    }
}
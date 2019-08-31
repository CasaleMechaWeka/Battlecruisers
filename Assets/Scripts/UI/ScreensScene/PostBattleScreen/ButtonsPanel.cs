using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class ButtonsPanel : MonoBehaviour
    {
        public virtual void Initialise(IPostBattleScreen postBattleScreen, ISoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(postBattleScreen, soundPlayer);

            ActionButton homeButton = transform.FindNamedComponent<ActionButton>("SmallButtons/HomeButton");
            homeButton.Initialise(postBattleScreen.GoToHomeScreen);
        }
    }
}
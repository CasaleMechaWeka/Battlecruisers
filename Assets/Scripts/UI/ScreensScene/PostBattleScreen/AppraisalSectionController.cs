using BattleCruisers.UI.ScreensScene.PostBattleScreen.States;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class AppraisalSectionController : MonoBehaviour
    {
        public Text appraisalDroneText;
        public ActionButton lootButton;

        public void Initialise(string appraisalText, ISingleSoundPlayer soundPlayer, VictoryState victoryState)
        {
            Helper.AssertIsNotNull(appraisalDroneText, lootButton);
            Helper.AssertIsNotNull(soundPlayer, victoryState);

            appraisalDroneText.text = appraisalText;
            lootButton.Initialise(soundPlayer, victoryState.ShowLoot);
        }
    }
}
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class AppraisalSectionController : MonoBehaviour
    {
        public Text appraisalDroneText;
        public ActionButton lootButton;

        public void Initialise(string appraisalText, ISingleSoundPlayer soundPlayer, Action lootButtonAction = null)
        {
            Helper.AssertIsNotNull(appraisalDroneText, lootButton);
            Assert.IsNotNull(soundPlayer);

            appraisalDroneText.text = appraisalText;

            if (lootButtonAction != null)
            {
                lootButton.Initialise(soundPlayer, lootButtonAction);
            }
            else
            {
                lootButton.gameObject.SetActive(false);
            }

            gameObject.SetActive(true);
        }
    }
}
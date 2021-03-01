using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class AppraisalSectionController : MonoBehaviour
    {
        private const string SIGNATURE_BASE = "- {0} <-@->";

        public Text appraisalDroneText;
        public Text appraisalDroneSignature;
        public CanvasGroupButton lootButton;

        public void Initialise(
            string appraisalText,
            ILocTable screensSceneStrings,
            ISingleSoundPlayer soundPlayer, 
            Action lootButtonAction = null)
        {
            Helper.AssertIsNotNull(appraisalDroneText, appraisalDroneSignature, lootButton);
            Helper.AssertIsNotNull(screensSceneStrings, soundPlayer);

            appraisalDroneText.text = appraisalText;

            string signature = screensSceneStrings.GetString("UI/PostBattleScreen/AppraisalDroneSignature");
            appraisalDroneSignature.text = string.Format(SIGNATURE_BASE, signature);

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
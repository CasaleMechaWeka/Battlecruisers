using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class AppraisalDroneTextButton : MonoBehaviour
    {
        private AppraisalSectionController _appraisalSection;
        private string _appraisalText;
        private ISingleSoundPlayer _soundPlayer;
        private ILocTable _screensSceneStrings;

        public Text levelNumText;

        public void Initialise(
            AppraisalSectionController appraisalSection, 
            string appraisalText, 
            ISingleSoundPlayer soundPlayer,
            int levelNum,
            ILocTable screensSceneStrings)
        {
            Assert.IsNotNull(levelNumText);
            Helper.AssertIsNotNull(appraisalSection, soundPlayer, screensSceneStrings);

            _appraisalSection = appraisalSection;
            _appraisalText = appraisalText;
            _soundPlayer = soundPlayer;
            levelNumText.text = levelNum.ToString();
            _screensSceneStrings = screensSceneStrings;
        }

        public void ShowAppraisalText()
        {
            _appraisalSection.Initialise(_appraisalText, _screensSceneStrings, _soundPlayer);
        }
    }
}
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
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

        public Text levelNumText;

        public void Initialise(
            AppraisalSectionController appraisalSection, 
            string appraisalText, 
            ISingleSoundPlayer soundPlayer,
            int levelNum)
        {
            Assert.IsNotNull(levelNumText);
            Helper.AssertIsNotNull(appraisalSection, soundPlayer);

            _appraisalSection = appraisalSection;
            _appraisalText = appraisalText;
            _soundPlayer = soundPlayer;
            levelNumText.text = levelNum.ToString();
        }

        public void ShowAppraisalText()
        {
            _appraisalSection.Initialise(_appraisalText, _soundPlayer);
        }
    }
}
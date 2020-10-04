using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class AppraisalButtonsPanel : MonoBehaviour
    {
        public async Task InitialiseAsync(
            AppraisalSectionController appraisalSection,
            ISingleSoundPlayer soundPlayer,
            ITrashTalkProvider trashTalkList)
        {
            Helper.AssertIsNotNull(appraisalSection, soundPlayer, trashTalkList);

            AppraisalDroneTextButton[] buttons = GetComponentsInChildren<AppraisalDroneTextButton>();
            Assert.AreEqual(StaticData.NUM_OF_LEVELS, buttons.Length);

            for (int i = 0; i < buttons.Length; ++i)
            {
                int levelNum = i + 1;
                ITrashTalkData trashData = await trashTalkList.GetTrashTalkAsync(levelNum);
                buttons[i]
                    .Initialise(
                        appraisalSection,
                        trashData.AppraisalDroneText,
                        soundPlayer,
                        levelNum);
            }
        }
    }
}
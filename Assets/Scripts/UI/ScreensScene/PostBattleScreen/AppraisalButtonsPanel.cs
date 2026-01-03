using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class AppraisalButtonsPanel : MonoBehaviour
    {
        public async Task InitialiseAsync(
            AppraisalSectionController appraisalSection,
            SingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(appraisalSection, soundPlayer);

            AppraisalDroneTextButton[] buttons = GetComponentsInChildren<AppraisalDroneTextButton>();
            Assert.AreEqual(StaticData.NUM_OF_LEVELS, buttons.Length);

            for (int i = 0; i < buttons.Length; ++i)
            {
                int levelNum = i + 1;
                TrashTalkData trashData = StaticData.LevelTrashTalk[levelNum];
                buttons[i]
                    .Initialise(
                        appraisalSection,
                        LocTableCache.StoryTable.GetString(trashData.AppraisalDroneTextKey),
                        soundPlayer,
                        levelNum);
            }
            await Task.CompletedTask;
        }
    }
}
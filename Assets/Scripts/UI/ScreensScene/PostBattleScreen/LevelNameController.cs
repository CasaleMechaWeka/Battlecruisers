using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class LevelNameController : MonoBehaviour
    {
        public void Initialise(IApplicationModel appModel, ITrashTalkDataList trashTalkList)
        {
            Helper.AssertIsNotNull(appModel, trashTalkList);

            if (appModel.IsTutorial)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);

                Text levelName = GetComponent<Text>();
                Assert.IsNotNull(levelName);
                levelName.text = CreateLevelName(appModel.DataProvider.GameModel.LastBattleResult, trashTalkList);
            }
        }

        private string CreateLevelName(BattleResult result, ITrashTalkDataList trashTalkList)
        {
            int levelNum = result.LevelNum;
            ITrashTalkData trashTalkData = trashTalkList.GetTrashTalk(levelNum);
            return $"#{levelNum} {trashTalkData.EnemyName}";
        }
    }
}
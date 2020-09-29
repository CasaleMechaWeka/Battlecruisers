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
        public void Initialise(IApplicationModel appModel, ITrashTalkData levelTrashTalkData)
        {
            Helper.AssertIsNotNull(appModel, levelTrashTalkData);

            if (appModel.IsTutorial)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);

                Text levelName = GetComponent<Text>();
                Assert.IsNotNull(levelName);
                levelName.text = CreateLevelName(appModel.DataProvider.GameModel.LastBattleResult, levelTrashTalkData);
            }
        }

        private string CreateLevelName(BattleResult result, ITrashTalkData levelTrashTalkData)
        {
            int levelNum = result.LevelNum;
            return $"#{levelNum} {levelTrashTalkData.EnemyName}";
        }
    }
}
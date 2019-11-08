using BattleCruisers.Data;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class LevelNameController : MonoBehaviour
    {
        public void Initialise(IApplicationModel appModel)
        {
            Assert.IsNotNull(appModel);

            if (appModel.IsTutorial)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);

                Text levelName = GetComponent<Text>();
                Assert.IsNotNull(levelName);
                levelName.text = CreateLevelName(appModel);
            }
        }

        private string CreateLevelName(IApplicationModel appModel)
        {
            int levelNum = appModel.DataProvider.GameModel.LastBattleResult.LevelNum;
            ILevel level = appModel.DataProvider.GetLevel(levelNum);
            return $"#{levelNum} {level.Name}";
        }
    }
}
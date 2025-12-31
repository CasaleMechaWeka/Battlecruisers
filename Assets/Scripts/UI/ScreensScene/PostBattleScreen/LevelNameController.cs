using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class LevelNameController : MonoBehaviour
    {
        public Text levelName;

        public void Initialise(int levelNum)
        {
            Assert.IsNotNull(levelName);

            levelName.text = $"#{levelNum} {LocTableCache.StoryTable.GetString(StaticData.LevelTrashTalk[levelNum].EnemyNameKey)}";
            gameObject.SetActive(true);
        }
    }
}
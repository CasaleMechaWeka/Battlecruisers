using BattleCruisers.UI.ScreensScene.TrashScreen;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class LevelNameController : MonoBehaviour
    {
        public Text levelName;

        public void Initialise(int levelNum, ITrashTalkData levelTrashTalkData)
        {
            Assert.IsNotNull(levelName);
            Assert.IsNotNull(levelTrashTalkData);

            levelName.text = $"#{levelNum} {levelTrashTalkData.EnemyName}";
        }
    }
}
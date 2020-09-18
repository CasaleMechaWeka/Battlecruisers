using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Scenes.Test.UI
{
    // FELIX  Rename, so can tell apart in inspector :P
    public class TrashScreenLevelButtonController : MonoBehaviour
    {
        private LevelButtonsPanel _levelButtonsPanel;
        private int _levelNum;
        private ITrashTalkData _trashTalkData;

        public Text levelNumText;

        public void Initialise(LevelButtonsPanel levelButtonsPanel, int levelNum, ITrashTalkData trashTalkData)
        {
            Assert.IsNotNull(levelNumText);
            Helper.AssertIsNotNull(levelButtonsPanel, trashTalkData);

            _levelButtonsPanel = levelButtonsPanel;
            _levelNum = levelNum;
            _trashTalkData = trashTalkData;

            levelNumText.text = levelNum.ToString();
        }

        public void ChangeLevel()
        {
            _levelButtonsPanel.ChangeLevelAsync(_levelNum, _trashTalkData);
        }
    }
}
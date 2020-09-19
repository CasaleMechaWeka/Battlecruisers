using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Scenes.Test.UI
{
    public class TrashScreenLevelButtonController : MonoBehaviour
    {
        private LevelButtonsPanel _levelButtonsPanel;
        private int _levelNum;

        public Text levelNumText;

        public void Initialise(LevelButtonsPanel levelButtonsPanel, int levelNum)
        {
            Assert.IsNotNull(levelNumText);
            Assert.IsNotNull(levelButtonsPanel);

            _levelButtonsPanel = levelButtonsPanel;
            _levelNum = levelNum;

            levelNumText.text = levelNum.ToString();
        }

        public void ChangeLevel()
        {
            _levelButtonsPanel.ChangeLevel(_levelNum);
        }
    }
}
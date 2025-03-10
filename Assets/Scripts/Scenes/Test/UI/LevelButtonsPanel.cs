using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.UI
{
    public class LevelButtonsPanel : MonoBehaviour
    {
        private IApplicationModel _appModel;
        private TrashScreenController _trashScreen;

        public void Initialise(IApplicationModel appModel, TrashScreenController trashScreen, int startingLevelNum)
        {
            Helper.AssertIsNotNull(appModel, trashScreen);
            Assert.IsTrue(startingLevelNum > 0);
            Assert.IsTrue(startingLevelNum <= StaticData.NUM_OF_LEVELS);

            _trashScreen = trashScreen;
            _appModel = appModel;

            TrashScreenLevelButtonController[] levelButtons = GetComponentsInChildren<TrashScreenLevelButtonController>();
            Assert.AreEqual(StaticData.NUM_OF_LEVELS, levelButtons.Length);

            for (int i = 0; i < levelButtons.Length; ++i)
            {
                int levelNum = i + 1;
                TrashScreenLevelButtonController levelButton = levelButtons[i];

                levelButton.Initialise(this, levelNum);

                if (levelNum == startingLevelNum)
                {
                    levelButton.ChangeLevel();
                }
            }
        }

        public void ChangeLevel(int levelNum)
        {
            _appModel.SelectedLevel = levelNum;
            _trashScreen.OnPresenting(activationParameter: null);
        }
    }
}
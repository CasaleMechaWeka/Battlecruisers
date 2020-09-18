using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.UI
{
    public class LevelButtonsPanel : MonoBehaviour
    {
        private TrashScreenController _trashScreen;
        private IPrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;
        private IScreensSceneGod _screensSceneGod;
        private IList<ILevel> _levels;
        private HullKey _playerCruiser;
        private ISpriteFetcher _spriteFetcher;

        public void Initialise(
            TrashScreenController trashScreen, 
            IPrefabFactory prefabFactory, 
            int startingLevelNum,
            HullKey playerCruiser,
            ITrashTalkDataList trashDataList)
        {
            Helper.AssertIsNotNull(trashScreen, prefabFactory, playerCruiser, trashDataList);
            Assert.IsTrue(startingLevelNum > 0);
            Assert.IsTrue(startingLevelNum <= StaticData.NUM_OF_LEVELS);

            _trashScreen = trashScreen;
            _prefabFactory = prefabFactory;
            _soundPlayer = Substitute.For<ISingleSoundPlayer>();
            _screensSceneGod = Substitute.For<IScreensSceneGod>();
            _levels = ApplicationModelProvider.ApplicationModel.DataProvider.Levels;
            Assert.AreEqual(StaticData.NUM_OF_LEVELS, _levels.Count);
            _playerCruiser = playerCruiser;
            _spriteFetcher = new SpriteFetcher();

            TrashScreenLevelButtonController[] levelButtons = GetComponentsInChildren<TrashScreenLevelButtonController>();
            Assert.AreEqual(StaticData.NUM_OF_LEVELS, levelButtons.Length);

            for (int i = 0; i < levelButtons.Length; ++i)
            {
                int levelNum = i + 1;
                ITrashTalkData trashData = trashDataList.GetTrashTalk(levelNum);
                TrashScreenLevelButtonController levelButton = levelButtons[i];

                levelButton.Initialise(this, levelNum, trashData);

                if (levelNum == startingLevelNum)
                {
                    levelButton.ChangeLevel();
                }
            }
        }

        public async Task ChangeLevelAsync(int levelNum, ITrashTalkData trashTalkData)
        {
            Assert.IsTrue(levelNum > 0);
            Assert.IsTrue(levelNum <= StaticData.NUM_OF_LEVELS);
            Assert.IsNotNull(trashTalkData);

            ILevel level = _levels[levelNum - 1];

            await _trashScreen
                .InitialiseAsync(
                    _soundPlayer,
                    _screensSceneGod,
                    trashTalkData,
                    level,
                    _prefabFactory,
                    _playerCruiser,
                    _spriteFetcher);
        }
    }
}
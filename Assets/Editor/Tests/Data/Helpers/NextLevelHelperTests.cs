using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Models;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Data.Helpers
{
    public class NextLevelHelperTests
    {
        private INextLevelHelper _helper;
        private IApplicationModel _appModel;
        private BattleResult _battleResult;

        [SetUp]
        public void TestSetup()
        {
            _appModel = Substitute.For<IApplicationModel>();

            _helper = new NextLevelHelper(_appModel);

            _battleResult = new BattleResult(levelNum: 2, wasVictory: true);
            _appModel.DataProvider.GameModel.LastBattleResult.Returns(_battleResult);

            _appModel.SelectedLevel.Returns(ApplicationModel.DEFAULT_SELECTED_LEVEL);
        }

        [Test]
        public void FindNextLevel_SelectedLevelExists()
        {
            int selectedLevel = 7;
            _appModel.SelectedLevel.Returns(selectedLevel);

            int nextLevel = _helper.FindNextLevel();

            Assert.AreEqual(selectedLevel, nextLevel);
        }

        [Test]
        public void FindNextLevel_NoSelectedLevelExists_BattleResultNotNull_Victory_LevelUnlocked()
        {
            _battleResult.WasVictory = true;
            int numOfLevelsUnlocked = _battleResult.LevelNum + 1;
            _appModel.DataProvider.LockedInfo.NumOfLevelsUnlocked.Returns(numOfLevelsUnlocked);

            int nextLevel = _helper.FindNextLevel();

            Assert.AreEqual(numOfLevelsUnlocked, nextLevel);
        }

        [Test]
        public void FindNextLevel_NoSelectedLevelExists_BattleResultNotNull_Victory_LevelLocked()
        {
            _battleResult.WasVictory = true;
            int numOfLevelsUnlocked = _battleResult.LevelNum;
            _appModel.DataProvider.LockedInfo.NumOfLevelsUnlocked.Returns(numOfLevelsUnlocked);

            int nextLevel = _helper.FindNextLevel();

            Assert.AreEqual(_battleResult.LevelNum, nextLevel);
        }

        [Test]
        public void FindNextLevel_NoSelectedLevelExists_BattleResultNotNull_Defeat()
        {
            _battleResult.WasVictory = false;

            int nextLevel = _helper.FindNextLevel();

            Assert.AreEqual(_battleResult.LevelNum, nextLevel);
        }

        [Test]
        public void FindNextLevel_NoSelectedLevelExists_BattleResultNull()
        {
            _appModel.DataProvider.GameModel.LastBattleResult.Returns((BattleResult)null);

            int nextLevel = _helper.FindNextLevel();

            Assert.AreEqual(1, nextLevel);
        }
    }
}
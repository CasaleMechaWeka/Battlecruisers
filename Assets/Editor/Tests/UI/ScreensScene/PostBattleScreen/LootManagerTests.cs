using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Data;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.UI.ScreensScene.PostBattleScreen;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.ScreensScene.PostBattleScreen
{
    public class LootManagerTests
    {
        private ILootManager _lootManager;

        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        private IItemDetailsGroup _middleDetailsGroup, _leftDetailsGroup, _rightDetailsGroup;

        private ILoot _unlockedLoot;
        private IList<ILootItem> _lootItems;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _dataProvider = Substitute.For<IDataProvider>();
            _prefabFactory = Substitute.For<IPrefabFactory>();
            _middleDetailsGroup = Substitute.For<IItemDetailsGroup>();
            _leftDetailsGroup = Substitute.For<IItemDetailsGroup>();
            _rightDetailsGroup = Substitute.For<IItemDetailsGroup>();

            _lootManager = new LootManager(_dataProvider, _prefabFactory, _middleDetailsGroup, _leftDetailsGroup, _rightDetailsGroup);

            _lootItems = new List<ILootItem>();
            ReadOnlyCollection<ILootItem> readonlyLootItems = new ReadOnlyCollection<ILootItem>(_lootItems);
            _unlockedLoot = Substitute.For<ILoot>();
            _unlockedLoot.Items.Returns(readonlyLootItems);

            _dataProvider.StaticData.GetLevelLoot(default(int)).ReturnsForAnyArgs(_unlockedLoot);
        }

        #region ShouldShowLoot
        [Test]
        public void ShouldShowLoot_True()
        {
            int levelCompleted = 7;
            _dataProvider.GameModel.NumOfLevelsCompleted = levelCompleted - 1;
            Assert.IsTrue(_lootManager.ShouldShowLoot(levelCompleted));
        }

        [Test]
        public void ShouldShowLoot_False()
        {
            int levelCompleted = 7;
            _dataProvider.GameModel.NumOfLevelsCompleted = levelCompleted;
            Assert.IsFalse(_lootManager.ShouldShowLoot(levelCompleted));
        }
        #endregion ShouldShowLoot

        #region UnlockLoot
        [Test]
        public void UnlockLoot_InvalidLevel_NotGreaterThanCurrent_Throws()
        {
            int levelCompleted = 7;
            _dataProvider.GameModel.NumOfLevelsCompleted = levelCompleted;
            Assert.Throws<UnityAsserts.AssertionException>(() => _lootManager.UnlockLoot(levelCompleted));
        }

        [Test]
        public void UnlockLoot_InvalidLevel_NotJustOneGreaterThanCurrent_Throws()
        {
            int levelCompleted = 7;
            _dataProvider.GameModel.NumOfLevelsCompleted = levelCompleted;
            Assert.Throws<UnityAsserts.AssertionException>(() => _lootManager.UnlockLoot(levelCompleted + 2));
        }

        [Test]
        public void UnlockLoot_UnlocksLevel()
        {
            int levelCompleted = 7;
            _dataProvider.GameModel.NumOfLevelsCompleted = levelCompleted - 1;

            _lootManager.UnlockLoot(levelCompleted);

            Assert.AreEqual(levelCompleted, _dataProvider.GameModel.NumOfLevelsCompleted);
        }
        #endregion UnlockLoot
    }
}

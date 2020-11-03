using BattleCruisers.Data;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.PostBattleScreen;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        private ILootItem _item1, _item2;

        [SetUp]
        public void SetuUp()
        {
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

            _dataProvider.StaticData.GetLevelLoot(default).ReturnsForAnyArgs(_unlockedLoot);
            _dataProvider.StaticData.LastLevelWithLoot.Returns(99);

            _item1 = Substitute.For<ILootItem>();
            _item2 = Substitute.For<ILootItem>();
        }

        #region ShouldShowLoot
        [Test]
        public void ShouldShowLoot_HaveCompletedLevelBefore_ReturnsFalse()
        {
            int levelCompleted = 7;
            _dataProvider.GameModel.NumOfLevelsCompleted.Returns(levelCompleted);

            Assert.IsFalse(_lootManager.ShouldShowLoot(levelCompleted));
        }

        [Test]
        public void ShouldShowLoot_HaveNotCompletedLevelBefore_LevelDoesNotHaveLoot_ReturnsFalse()
        {
            int levelCompleted = 7;
            _dataProvider.GameModel.NumOfLevelsCompleted.Returns(levelCompleted - 1);
            _dataProvider.StaticData.LastLevelWithLoot.Returns(levelCompleted - 1);

            Assert.IsFalse(_lootManager.ShouldShowLoot(levelCompleted));
        }

        [Test]
        public void ShouldShowLoot_HaveNotCompletedLevelBefore_LevelHasLoot_ReturnsTrue()
        {
            int levelCompleted = 7;
            _dataProvider.GameModel.NumOfLevelsCompleted.Returns(levelCompleted - 1);
            _dataProvider.StaticData.LastLevelWithLoot.Returns(levelCompleted);

            Assert.IsTrue(_lootManager.ShouldShowLoot(levelCompleted));
        }
        #endregion ShouldShowLoot

        [Test]
        public void UnlockLoot_UnlocksLootItems()
        {
            _lootItems.Add(_item1);
            _lootItems.Add(_item2);

            TriggerUnlock();

            _item1.Received().UnlockItem(_dataProvider.GameModel);
            _item2.Received().UnlockItem(_dataProvider.GameModel);
        }

        #region ShowLoot
        [Test]
        public void ShowLoot_InvalidItemCount_Throws()
        {
            // Should only ever have <= 2 loot items, not 3
            _lootItems.Add(_item1);
            _lootItems.Add(_item1);
            _lootItems.Add(_item1);

            Assert.Throws<ArgumentException>(() => _lootManager.ShowLoot(_unlockedLoot));
        }

        [Test]
        public void ShowLoot_SingleItem()
        {
            _lootItems.Add(_item1);

            _lootManager.ShowLoot(_unlockedLoot);

            _item1.Received().ShowItemDetails(_prefabFactory, _middleDetailsGroup);
        }

        [Test]
        public void ShowLoot_TwoItems()
        {
            _lootItems.Add(_item1);
            _lootItems.Add(_item2);

            _lootManager.ShowLoot(_unlockedLoot);

            _item1.Received().ShowItemDetails(_prefabFactory, _leftDetailsGroup);
            _item2.Received().ShowItemDetails(_prefabFactory, _rightDetailsGroup);
        }
        #endregion ShowLoot

        private void TriggerUnlock()
        {
            int levelCompleted = 7;
            _dataProvider.GameModel.NumOfLevelsCompleted.Returns(levelCompleted - 1);
            _dataProvider.StaticData.LastLevelWithLoot.Returns(levelCompleted);

            _lootManager.UnlockLoot(levelCompleted);
        }
    }
}

using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.PostBattleScreen;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Tests.UI.ScreensScene.PostBattleScreen
{
    public class LootManagerTests
    {
        private LootManager _lootManager;

        private ItemDetailsGroupController _middleDetailsGroup, _leftDetailsGroup, _rightDetailsGroup;

        private Loot _unlockedLoot;
        private IList<ILootItem> _lootItems;
        private ILootItem _item1, _item2;

        [SetUp]
        public void SetuUp()
        {
            _middleDetailsGroup = Substitute.For<ItemDetailsGroupController>();
            _leftDetailsGroup = Substitute.For<ItemDetailsGroupController>();
            _rightDetailsGroup = Substitute.For<ItemDetailsGroupController>();

            _lootManager = new LootManager(_middleDetailsGroup, _leftDetailsGroup, _rightDetailsGroup);

            _lootItems = new List<ILootItem>();
            ReadOnlyCollection<ILootItem> readonlyLootItems = new ReadOnlyCollection<ILootItem>(_lootItems);
            _unlockedLoot = Substitute.For<Loot>();
            _unlockedLoot.Items.Returns(readonlyLootItems);

            StaticData.GetLevelLoot(default).ReturnsForAnyArgs(_unlockedLoot);
            StaticData.LastLevelWithLoot.Returns(99);

            _item1 = Substitute.For<ILootItem>();
            _item2 = Substitute.For<ILootItem>();
        }

        #region ShouldShowLoot
        [Test]
        public void ShouldShowLoot_HaveCompletedLevelBefore_ReturnsFalse()
        {
            int levelCompleted = 7;
            DataProvider.GameModel.NumOfLevelsCompleted.Returns(levelCompleted);

            Assert.IsFalse(_lootManager.ShouldShowLevelLoot(levelCompleted));
        }

        [Test]
        public void ShouldShowLoot_HaveNotCompletedLevelBefore_LevelDoesNotHaveLoot_ReturnsFalse()
        {
            int levelCompleted = 7;
            DataProvider.GameModel.NumOfLevelsCompleted.Returns(levelCompleted - 1);
            StaticData.LastLevelWithLoot.Returns(levelCompleted - 1);

            Assert.IsFalse(_lootManager.ShouldShowLevelLoot(levelCompleted));
        }

        [Test]
        public void ShouldShowLoot_HaveNotCompletedLevelBefore_LevelHasLoot_ReturnsTrue()
        {
            int levelCompleted = 7;
            DataProvider.GameModel.NumOfLevelsCompleted.Returns(levelCompleted - 1);
            StaticData.LastLevelWithLoot.Returns(levelCompleted);

            Assert.IsTrue(_lootManager.ShouldShowLevelLoot(levelCompleted));
        }
        #endregion ShouldShowLoot

        [Test]
        public void UnlockLoot_UnlocksLootItems()
        {
            _lootItems.Add(_item1);
            _lootItems.Add(_item2);

            TriggerUnlock();

            _item1.Received().UnlockItem(DataProvider.GameModel);
            _item2.Received().UnlockItem(DataProvider.GameModel);
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

            _item1.Received().ShowItemDetails(_middleDetailsGroup);
        }

        [Test]
        public void ShowLoot_TwoItems()
        {
            _lootItems.Add(_item1);
            _lootItems.Add(_item2);

            _lootManager.ShowLoot(_unlockedLoot);

            _item1.Received().ShowItemDetails(_leftDetailsGroup);
            _item2.Received().ShowItemDetails(_rightDetailsGroup);
        }
        #endregion ShowLoot

        private void TriggerUnlock()
        {
            int levelCompleted = 7;
            DataProvider.GameModel.NumOfLevelsCompleted.Returns(levelCompleted - 1);
            StaticData.LastLevelWithLoot.Returns(levelCompleted);

            LootManager.UnlockLevelLoot(levelCompleted);
        }
    }
}

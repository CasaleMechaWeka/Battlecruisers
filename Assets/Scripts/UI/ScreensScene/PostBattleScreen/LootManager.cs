using BattleCruisers.Data;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class LootManager : ILootManager
    {
        private readonly DataProvider _dataProvider;
        private readonly PrefabFactory _prefabFactory;
        private readonly IItemDetailsGroup _middleDetailsGroup, _leftDetailsGroup, _rightDetailsGroup;

        public LootManager(
            DataProvider dataProvider,
            PrefabFactory prefabFactory,
            IItemDetailsGroup middleDetailsGroup,
            IItemDetailsGroup leftDetailsGroup,
            IItemDetailsGroup rightDetailsGroup)
        {
            Helper.AssertIsNotNull(dataProvider, prefabFactory, middleDetailsGroup, leftDetailsGroup, rightDetailsGroup);

            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _middleDetailsGroup = middleDetailsGroup;
            _leftDetailsGroup = leftDetailsGroup;
            _rightDetailsGroup = rightDetailsGroup;
        }

        public bool ShouldShowLevelLoot(int levelCompleted)
        {
            ILoot unlockedLoot = _dataProvider.StaticData.GetLevelLoot(levelCompleted);
            bool containsNewLoot = false;
            if (unlockedLoot.Items.Count != 0)
                for (int i = 0; i < unlockedLoot.Items.Count; i++)
                    if (!unlockedLoot.Items[i].IsUnlocked(_dataProvider.GameModel))
                        containsNewLoot = true;

            return
                containsNewLoot || (levelCompleted > _dataProvider.GameModel.NumOfLevelsCompleted
                && levelCompleted <= _dataProvider.StaticData.LastLevelWithLoot);
        }

        //have to do when SideQuest data is stored in StaticData
        public bool ShouldShowSideQuestLoot(int sideQuestID)
        {
            ILoot unlockedLoot = _dataProvider.StaticData.GetSideQuestLoot(sideQuestID);

            bool containsNewLoot = unlockedLoot.Items.Any(item => !item.IsUnlocked(_dataProvider.GameModel));

            // If there are no completed sidequests recorded, simply return based on new loot found.
            if (_dataProvider.GameModel.CompletedSideQuests == null ||
                !_dataProvider.GameModel.CompletedSideQuests.Any())
            {
                return containsNewLoot;
            }

            List<int> completedSideQuestIDs = _dataProvider.GameModel.CompletedSideQuests
                .Select(completedSQ => completedSQ.LevelNum)  // Ensure LevelNum here really represents sidequest ID.
                .ToList();

            return containsNewLoot || !completedSideQuestIDs.Contains(sideQuestID);
        }

        public ILoot UnlockLevelLoot(int levelCompleted)
        {
            Debug.Log($"UnlockLevelLoot called for level: {levelCompleted}");
            ILoot unlockedLoot = _dataProvider.StaticData.GetLevelLoot(levelCompleted);

            if (unlockedLoot.Items.Count != 0)
            {
                UnlockLootItems(unlockedLoot);
                _dataProvider.SaveGame();
            }

            return unlockedLoot;
        }

        public ILoot UnlockSideQuestLoot(int sideQuestID)
        {
            Debug.Log($"UnlockSideQuestLoot called for sideQuestID: {sideQuestID}");
            ILoot unlockedLoot = _dataProvider.StaticData.GetSideQuestLoot(sideQuestID);

            if (unlockedLoot.Items.Count != 0)
            {
                UnlockLootItems(unlockedLoot);
                _dataProvider.SaveGame();
            }

            return unlockedLoot;
        }

        private void UnlockLootItems(ILoot unlockedLoot)
        {
            foreach (ILootItem lootItem in unlockedLoot.Items)
            {
                lootItem.UnlockItem(_dataProvider.GameModel);
            }
        }

        public void ShowLoot(ILoot unlockedLoot)
        {
            Assert.IsNotNull(unlockedLoot);

            switch (unlockedLoot.Items.Count)
            {
                case 1:
                    // Show item details in middle of screen
                    unlockedLoot.Items[0].ShowItemDetails(_prefabFactory, _middleDetailsGroup);
                    break;

                case 2:
                    // Show item details to left and right sides of screen
                    unlockedLoot.Items[0].ShowItemDetails(_prefabFactory, _leftDetailsGroup);
                    unlockedLoot.Items[1].ShowItemDetails(_prefabFactory, _rightDetailsGroup);
                    break;

                default:
                    throw new ArgumentException();
            }
        }
    }
}

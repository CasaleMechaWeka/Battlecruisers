using BattleCruisers.Data;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using BattleCruisers.Data.Static;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class LootManager
    {
        private readonly ItemDetailsGroupController _middleDetailsGroup, _leftDetailsGroup, _rightDetailsGroup;

        public LootManager(
            ItemDetailsGroupController middleDetailsGroup,
            ItemDetailsGroupController leftDetailsGroup,
            ItemDetailsGroupController rightDetailsGroup)
        {
            Helper.AssertIsNotNull(middleDetailsGroup, leftDetailsGroup, rightDetailsGroup);

            _middleDetailsGroup = middleDetailsGroup;
            _leftDetailsGroup = leftDetailsGroup;
            _rightDetailsGroup = rightDetailsGroup;
        }

        public bool ShouldShowLevelLoot(int levelCompleted)
        {
            ILoot unlockedLoot = StaticData.GetLevelLoot(levelCompleted);
            bool containsNewLoot = false;
            if (unlockedLoot.Items.Count != 0)
                for (int i = 0; i < unlockedLoot.Items.Count; i++)
                    if (!unlockedLoot.Items[i].IsUnlocked(DataProvider.GameModel))
                        containsNewLoot = true;

            return
                containsNewLoot || (levelCompleted > DataProvider.GameModel.NumOfLevelsCompleted
                && levelCompleted <= StaticData.LastLevelWithLoot);
        }

        //have to do when SideQuest data is stored in StaticData
        public bool ShouldShowSideQuestLoot(int sideQuestID)
        {
            ILoot unlockedLoot = StaticData.GetSideQuestLoot(sideQuestID);

            bool containsNewLoot = unlockedLoot.Items.Any(item => !item.IsUnlocked(DataProvider.GameModel));

            // If there are no completed sidequests recorded, simply return based on new loot found.
            if (DataProvider.GameModel.CompletedSideQuests == null ||
                !DataProvider.GameModel.CompletedSideQuests.Any())
            {
                return containsNewLoot;
            }

            List<int> completedSideQuestIDs = DataProvider.GameModel.CompletedSideQuests
                .Select(completedSQ => completedSQ.LevelNum)  // Ensure LevelNum here really represents sidequest ID.
                .ToList();

            return containsNewLoot || !completedSideQuestIDs.Contains(sideQuestID);
        }

        public ILoot UnlockLevelLoot(int levelCompleted)
        {
            Debug.Log($"UnlockLevelLoot called for level: {levelCompleted}");
            ILoot unlockedLoot = StaticData.GetLevelLoot(levelCompleted);

            if (unlockedLoot.Items.Count != 0)
            {
                UnlockLootItems(unlockedLoot);
                DataProvider.SaveGame();
            }

            return unlockedLoot;
        }

        public ILoot UnlockSideQuestLoot(int sideQuestID)
        {
            Debug.Log($"UnlockSideQuestLoot called for sideQuestID: {sideQuestID}");
            ILoot unlockedLoot = StaticData.GetSideQuestLoot(sideQuestID);

            if (unlockedLoot.Items.Count != 0)
            {
                UnlockLootItems(unlockedLoot);
                DataProvider.SaveGame();
            }

            return unlockedLoot;
        }

        private void UnlockLootItems(ILoot unlockedLoot)
        {
            foreach (ILootItem lootItem in unlockedLoot.Items)
            {
                lootItem.UnlockItem(DataProvider.GameModel);
            }
        }

        public void ShowLoot(ILoot unlockedLoot)
        {
            Assert.IsNotNull(unlockedLoot);

            switch (unlockedLoot.Items.Count)
            {
                case 1:
                    // Show item details in middle of screen
                    unlockedLoot.Items[0].ShowItemDetails(_middleDetailsGroup);
                    break;

                case 2:
                    // Show item details to left and right sides of screen
                    unlockedLoot.Items[0].ShowItemDetails(_leftDetailsGroup);
                    unlockedLoot.Items[1].ShowItemDetails(_rightDetailsGroup);
                    break;

                default:
                    throw new ArgumentException();
            }
        }
    }
}

using BattleCruisers.Data;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class LootManager : ILootManager
    {
        private readonly IDataProvider _dataProvider;
        private readonly IPrefabFactory _prefabFactory;
        private readonly IItemDetailsGroup _middleDetailsGroup, _leftDetailsGroup, _rightDetailsGroup;

        public LootManager(
            IDataProvider dataProvider, 
            IPrefabFactory prefabFactory, 
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

        public bool ShouldShowLoot(int levelCompleted)
        {
            return 
                levelCompleted > _dataProvider.GameModel.NumOfLevelsCompleted
                && levelCompleted <= _dataProvider.StaticData.LastLevelWithLoot;
        }

        // FELIX  Update tests
        public ILoot UnlockLoot(int levelCompleted)
        {
            ILoot unlockedLoot = _dataProvider.StaticData.GetLevelLoot(levelCompleted);

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

using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class BuildingItemsCagetoryButton : ItemCategoryButton
    {
        protected override ItemFamily ItemFamily => ItemFamily.Buildings;

        protected override bool HasNewItems(IGameModel gameModel)
        {
            return gameModel.NewBuildings.Items.Any(buildingKey => BuildingCategoryToItemType(buildingKey.BuildingCategory) == itemType);
        }

        protected override void SetupNewMarkVisibilityCallback(IGameModel gameModel)
        {
            gameModel.NewBuildings.Items.Parse<INotifyCollectionChanged>().CollectionChanged += BuildingItemsCagetoryButton_CollectionChanged;
        }

        private void BuildingItemsCagetoryButton_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateNewItemMarkVisibility();
        }

        private ItemType BuildingCategoryToItemType(BuildingCategory buildingCategory)
        {
            switch (buildingCategory)
            {
                case BuildingCategory.Defence:
                    return ItemType.Defense;

                case BuildingCategory.Factory:
                    return ItemType.Factory;

                case BuildingCategory.Offence:
                    return ItemType.Offensive;

                case BuildingCategory.Tactical:
                    return ItemType.Tactical;

                case BuildingCategory.Ultra:
                    return ItemType.Ultra;

                default:
                    throw new ArgumentException($"Unknown building category: {buildingCategory}");
            }
        }

        protected override void CleanUp(IGameModel gameModel)
        {
            gameModel.NewBuildings.Items.Parse<INotifyCollectionChanged>().CollectionChanged -= BuildingItemsCagetoryButton_CollectionChanged;
        }
    }
}
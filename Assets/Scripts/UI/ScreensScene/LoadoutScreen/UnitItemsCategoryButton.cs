using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class UnitItemsCategoryButton : ItemCategoryButton
    {
        protected override ItemFamily ItemFamily => ItemFamily.Units;

        protected override bool HasNewItems(IGameModel gameModel)
        {
            return gameModel.NewUnits.Items.Any(unitKey => UnitCategoryToItemType(unitKey.UnitCategory) == itemType);
        }

        protected override void SetupNewMarkVisibilityCallback(IGameModel gameModel)
        {
            gameModel.NewUnits.Items.Parse<INotifyCollectionChanged>().CollectionChanged += UnitItemsCategoryButton_CollectionChanged;
        }

        private void UnitItemsCategoryButton_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateNewItemMarkVisibility();
        }

        private ItemType UnitCategoryToItemType(UnitCategory unitCategory)
        {
            switch (unitCategory)
            {
                case UnitCategory.Aircraft:
                    return ItemType.Aircraft;

                case UnitCategory.Naval:
                    return ItemType.Ship;

                default:
                    throw new ArgumentException($"Unsupported unit category: {unitCategory}");
            }
        }

        protected override void CleanUp(IGameModel gameModel)
        {
            gameModel.NewUnits.Items.Parse<INotifyCollectionChanged>().CollectionChanged -= UnitItemsCategoryButton_CollectionChanged;
        }
    }
}
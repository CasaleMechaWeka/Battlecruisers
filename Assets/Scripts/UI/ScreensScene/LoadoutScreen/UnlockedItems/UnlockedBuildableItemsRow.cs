using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
    public abstract class UnlockedBuildableItemsRow<TItem> : UnlockedItemsRow<TItem> where TItem : IBuildable
	{
        private IList<TItem> _loadoutBuildables;

		public virtual void Initialise(
            IItemsRow<TItem> itemsRow, 
            IUIFactory uiFactory, 
            IList<TItem> unlockedBuildables, 
            IList<TItem> loadoutBuildables, 
            IItemDetailsManager<TItem> detailsManager)
		{
            base.Initialise(uiFactory, unlockedBuildables, itemsRow, detailsManager);

            Assert.IsNotNull(loadoutBuildables);
            _loadoutBuildables = loadoutBuildables;
		}

        protected sealed override UnlockedItem<TItem> CreateUnlockedItem(TItem item, HorizontalOrVerticalLayoutGroup itemParent)
		{
			bool isInLoadout = _loadoutBuildables.Contains(item);
            return CreateUnlockedItem(item, itemParent, isInLoadout);
		}

        protected abstract UnlockedItem<TItem> CreateUnlockedItem(TItem item, HorizontalOrVerticalLayoutGroup itemParent, bool isInLoadout);
	}
}

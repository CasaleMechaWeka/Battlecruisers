using System.Collections.Generic;
using BattleCruisers.Buildables;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public abstract class UnlockedBuildableItemsRow<TItem> : UnlockedItemsRow<TItem> where TItem : IBuildable
	{
        private IList<TItem> _loadoutBuildables;

        public virtual void Initialise(IUnlockedItemsRowArgs<TItem> args, IList<TItem> loadoutBuildables)
		{
            base.Initialise(args);

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

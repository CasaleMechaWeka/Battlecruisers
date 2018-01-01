using BattleCruisers.Cruisers;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public class UnlockedHullItemsRow : UnlockedItemsRow<ICruiser>
	{
		private ICruiser _loadoutCruiser;

        public void Initialise(IUnlockedItemsRowArgs<ICruiser> args, ICruiser loadoutCruiser)
		{

            Assert.IsTrue(args.UnlockedItems.Count > 0);

            _loadoutCruiser = loadoutCruiser;

            // FELIX  Move to start of method.  Should be ok sue to SetupUI() :)
			base.Initialise(args);
		}

		protected override UnlockedItem<ICruiser> CreateUnlockedItem(ICruiser item, HorizontalOrVerticalLayoutGroup itemParent)
		{
			bool isInLoadout = object.ReferenceEquals(_loadoutCruiser, item);
			return _uiFactory.CreateUnlockedHull(layoutGroup, _itemsRow, item, isInLoadout);
		}

		public void UpdateSelectedHull(ICruiser selectedCruiser)
		{
			foreach (UnlockedHullItem unlockedHullButton in _unlockedItemButtons)
			{
				unlockedHullButton.OnNewHullSelected(selectedCruiser);
			}
		}
	}
}

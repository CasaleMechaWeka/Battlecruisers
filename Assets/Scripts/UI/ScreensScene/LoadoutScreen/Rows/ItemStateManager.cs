using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class ItemStateManager : IItemStateManager
    {
        private readonly IDictionary<ItemType, IList<IStatefulUIElement>> _typeToItems;
		
        public ItemStateManager()
        {
            _typeToItems = new Dictionary<ItemType, IList<IStatefulUIElement>>();
        }

        public void AddItem(IStatefulUIElement itemToAdd, ItemType type)
        {
            if (!_typeToItems.ContainsKey(type))
            {
                _typeToItems.Add(type, new List<IStatefulUIElement>());
            }

            IList<IStatefulUIElement> items = _typeToItems[type];
            Assert.IsFalse(items.Contains(itemToAdd), "Cannot add duplicate item.");
            items.Add(itemToAdd);
        }

		public void HandleDetailsManagerDismissed()
		{
            MakeAllItemsDefault();
		}
        
        public void HandleDetailsManagerComparing()
        {
            MakeAllItemsDefault();
        }

        private void MakeAllItemsDefault()
        {
            foreach (IList<IStatefulUIElement> items in _typeToItems.Values)
            {
                foreach (IStatefulUIElement item in items)
                {
                    item.GoToState(UIState.Default);
                }
            }
        }
		
        public void HandleDetailsManagerReadyToCompare(ItemType comparingType)
		{
            Assert.IsTrue(_typeToItems.ContainsKey(comparingType));

            HighlightComparableItems(comparingType);
            DisableIncomparableItems(comparingType);
        }

        private void DisableIncomparableItems(ItemType comparingType)
        {
            IEnumerable<IList<IStatefulUIElement>> incomparableItemLists
                = _typeToItems
                    .Where(typeToItem => typeToItem.Key != comparingType)
                    .Select(typeToItem => typeToItem.Value);

            foreach (IList<IStatefulUIElement> incomparableItemList in incomparableItemLists)
            {
                foreach (IStatefulUIElement incomparableItm in incomparableItemList)
                {
                    incomparableItm.GoToState(UIState.Disabled);
                }
            }
        }

        private void HighlightComparableItems(ItemType comparingType)
        {
            IList<IStatefulUIElement> comparableItems = _typeToItems[comparingType];
            foreach (IStatefulUIElement comparableItem in comparableItems)
            {
                comparableItem.GoToState(UIState.Highlighted);
            }
        }
    }
}

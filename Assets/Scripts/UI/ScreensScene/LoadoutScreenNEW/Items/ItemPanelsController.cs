using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
{
    public class ItemPanelsController : MonoBehaviour, IItemPanelsController
    {
        private IDictionary<ItemType, IItemsPanel> _typeToPanel;

        private IItemsPanel _currentlyShownPanel;
        private IItemsPanel CurrentlyShownPanel
        {
            set
            {
                if (_currentlyShownPanel != null)
                {
                    _currentlyShownPanel.Hide();
                }

                _currentlyShownPanel = value;

                if (_currentlyShownPanel != null)
                {
                    _currentlyShownPanel.Show();
                }
            }
        }

        public void Initialise(ItemType defaultItemTypeToShow)
        {
            _typeToPanel = new Dictionary<ItemType, IItemsPanel>();

            IItemsPanel[] panels = GetComponentsInChildren<IItemsPanel>(includeInactive: true);

            foreach (IItemsPanel panel in panels)
            {
                _typeToPanel.Add(panel.ItemType, panel);
                panel.Hide();

                if (defaultItemTypeToShow == panel.ItemType)
                {
                    CurrentlyShownPanel = panel;
                }
            }
        }

        public void ShowItemsPanel(ItemType itemType)
        {
            Assert.IsTrue(_typeToPanel.ContainsKey(itemType));
            CurrentlyShownPanel = _typeToPanel[itemType];
        }
    }
}
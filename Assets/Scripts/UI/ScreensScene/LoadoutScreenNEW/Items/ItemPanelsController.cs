using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.Utils;
using System;
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
            get { return _currentlyShownPanel; }
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

                if (PotentialMatchChange != null)
                {
                    PotentialMatchChange.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler PotentialMatchChange;

        public void Initialise(
            IItemDetailsManager itemDetailsManager, 
            ItemType defaultItemTypeToShow,
            IComparingItemFamilyTracker comparingFamiltyTracker)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamiltyTracker);

            _typeToPanel = new Dictionary<ItemType, IItemsPanel>();

            ItemsPanel[] panels = GetComponentsInChildren<ItemsPanel>(includeInactive: true);

            foreach (ItemsPanel panel in panels)
            {
                panel.Initialise(itemDetailsManager, comparingFamiltyTracker);
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

        public bool IsMatch(ItemType element)
        {
            return
                CurrentlyShownPanel != null
                && CurrentlyShownPanel.ItemType == element;
        }
    }
}
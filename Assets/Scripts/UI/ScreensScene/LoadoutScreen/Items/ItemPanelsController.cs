using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
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
            IComparingItemFamilyTracker comparingFamiltyTracker,
            IGameModel gameModel,
            IBroadcastingProperty<HullKey> selectedHull)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamiltyTracker, gameModel, selectedHull);

            _typeToPanel = new Dictionary<ItemType, IItemsPanel>();

            ItemsPanel[] panels = GetComponentsInChildren<ItemsPanel>(includeInactive: true);

            foreach (ItemsPanel panel in panels)
            {
                panel.Initialise(itemDetailsManager, comparingFamiltyTracker, gameModel, selectedHull);
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

        public IItemsPanel GetPanel(ItemType itemType)
        {
            Assert.IsTrue(_typeToPanel.ContainsKey(itemType));
            return _typeToPanel[itemType];
        }

        public bool IsMatch(ItemType element)
        {
            return
                CurrentlyShownPanel != null
                && CurrentlyShownPanel.ItemType == element;
        }
    }
}
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using BattleCruisers.Utils.Properties;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class ItemPanelsController : MonoBehaviour
    {
        private IDictionary<ItemType, ItemsPanel> _typeToPanel;

        private ItemsPanel _currentlyShownPanel;

        private List<ItemButton> _allItemButtons = new List<ItemButton>();
        public ItemsPanel CurrentlyShownPanel
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

                PotentialMatchChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler PotentialMatchChange;

        public IList<ItemButton> Initialise(
            ItemDetailsManager itemDetailsManager,
            ItemType defaultItemTypeToShow,
            ComparingItemFamilyTracker comparingFamiltyTracker,
            IBroadcastingProperty<HullKey> selectedHull,
            SingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamiltyTracker, selectedHull, soundPlayer);

            _typeToPanel = new Dictionary<ItemType, ItemsPanel>();

            ItemsPanel[] panels = GetComponentsInChildren<ItemsPanel>(includeInactive: true);
            List<ItemButton> allItemButtons = new List<ItemButton>();

            foreach (ItemsPanel panel in panels)
            {
                IList<ItemButton> panelItemButtons = panel.Initialise(itemDetailsManager, comparingFamiltyTracker, selectedHull, soundPlayer);
                allItemButtons.AddRange(panelItemButtons);
                _typeToPanel.Add(panel.ItemType, panel);
                panel.Hide();

                if (defaultItemTypeToShow == panel.ItemType)
                {
                    CurrentlyShownPanel = panel;
                }
            }
            _allItemButtons = allItemButtons;
            return allItemButtons;
        }

        public void AddHeckle(HeckleData heckleData)
        {
            ItemsPanel[] panels = GetComponentsInChildren<ItemsPanel>(includeInactive: true);
            panels[panels.Length - 1].AddHeckle(heckleData);
        }

        public void ShowItemsPanel(ItemType itemType)
        {
            Assert.IsTrue(_typeToPanel.ContainsKey(itemType));
            CurrentlyShownPanel = _typeToPanel[itemType];
            ItemsPanel itemsPanel = _typeToPanel[itemType];
            itemsPanel.GetFirstItemButton().ShowDetails();
        }

        public ItemsPanel GetPanel(ItemType itemType)
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

        public bool IsMatch(ItemType element, VariantPrefab variant)
        {
            return
                CurrentlyShownPanel != null
                && CurrentlyShownPanel.ItemType == element;
        }

        public void ShowHecklePanel()
        {
            CurrentlyShownPanel = null;
        }
    }
}
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System;
using System.Collections.Generic;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class ItemPanelsController : MonoBehaviour, IItemPanelsController
    {
        private IDictionary<ItemType, IItemsPanel> _typeToPanel;

        private IItemsPanel _currentlyShownPanel;
        public IItemsPanel CurrentlyShownPanel
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

        public IList<IItemButton> Initialise(
            IItemDetailsManager itemDetailsManager, 
            ItemType defaultItemTypeToShow,
            IComparingItemFamilyTracker comparingFamiltyTracker,
            IGameModel gameModel,
            IBroadcastingProperty<HullKey> selectedHull,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamiltyTracker, gameModel, selectedHull, soundPlayer, prefabFactory);

            _typeToPanel = new Dictionary<ItemType, IItemsPanel>();

            ItemsPanel[] panels = GetComponentsInChildren<ItemsPanel>(includeInactive: true);
            List<IItemButton> allItemButtons = new List<IItemButton>();

            foreach (ItemsPanel panel in panels)
            {
                IList<IItemButton> panelItemButtons = panel.Initialise(itemDetailsManager, comparingFamiltyTracker, gameModel, selectedHull, soundPlayer, prefabFactory);
                allItemButtons.AddRange(panelItemButtons);
                _typeToPanel.Add(panel.ItemType, panel);
                panel.Hide();

                if (defaultItemTypeToShow == panel.ItemType)
                {
                    CurrentlyShownPanel = panel;
                }
            }

            return allItemButtons;
        }

        public void ShowItemsPanel(ItemType itemType)
        {
            Assert.IsTrue(_typeToPanel.ContainsKey(itemType));
            CurrentlyShownPanel = _typeToPanel[itemType];
            IItemsPanel itemsPanel = _typeToPanel[itemType];
            itemsPanel.GetFirstItemButton().ShowDetails();
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
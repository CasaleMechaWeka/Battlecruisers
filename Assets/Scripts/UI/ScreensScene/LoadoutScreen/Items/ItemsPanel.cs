using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System.Collections.Generic;
using BattleCruisers.Utils.Properties;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using UnityEngine;
using System;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class ItemsPanel : Panel, IItemsPanel
    {
        private IList<IItemButton> _button;

        public ItemType itemType;
        public ItemType ItemType => itemType;

        public bool HasUnlockedItem { get; private set; }

        public HeckleItemContainerV2 HeckleItemContainerV2Prefab;
        public Transform heckleParent;
        public SelectHeckleButton toggleHeckleSelectionButton;
        private IList<IItemButton> buttons = new List<IItemButton>();
        private IItemDetailsManager _itemDetailsManager;
        private IComparingItemFamilyTracker _comparingFamiltyTracker;
        private IBroadcastingProperty<HullKey> _selectedHull;
        private ISingleSoundPlayer _soundPlayer;
        // Heckle Logic

        private HeckleButtonV2 _currentHeckleButton;
        public HeckleButtonV2 CurrentHeckleButton
        {
            set
            {
                _currentHeckleButton = value;
                HeckleButtonChanged?.Invoke(this, EventArgs.Empty);
            }
            get
            {
                return _currentHeckleButton;
            }
        }

        public EventHandler HeckleButtonChanged;

        public void AddHeckle(HeckleData heckleData)
        {
            if (DataProvider.GameModel.PurchasedHeckles.Contains(heckleData.Index))
            {
                HeckleItemContainerV2 heckleContainer = Instantiate(HeckleItemContainerV2Prefab, heckleParent);
                heckleContainer.heckleData = heckleData;
                heckleContainer.toggleSelectionButton = toggleHeckleSelectionButton;
                IItemButton button = heckleContainer.Initialise(this, _itemDetailsManager, _comparingFamiltyTracker, DataProvider.GameModel, _selectedHull, _soundPlayer);
                buttons.Add(button);
                heckleContainer.gameObject.SetActive(true);
            }
        }

        public IList<IItemButton> Initialise(
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingFamiltyTracker,
            IBroadcastingProperty<HullKey> selectedHull,
            ISingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamiltyTracker, selectedHull);

            _itemDetailsManager = itemDetailsManager;
            _comparingFamiltyTracker = comparingFamiltyTracker;
            _selectedHull = selectedHull;
            _soundPlayer = soundPlayer;

            if (this == null)
                return null;
            if (itemType == ItemType.Heckle)
            {
                HasUnlockedItem = true;
                foreach (HeckleData heckleData in StaticData.Heckles)
                {
                    AddHeckle(heckleData);
                }
                _button = buttons;
                return buttons;
            }
            else
            {
                if (this == null)
                    return null;
                ItemContainer[] itemContainers = GetComponentsInChildren<ItemContainer>(includeInactive: true);
                IList<IItemButton> buttons = new List<IItemButton>();

                HasUnlockedItem = false;

                foreach (ItemContainer itemContainer in itemContainers)
                {
                    IItemButton button = itemContainer.Initialise(this, itemDetailsManager, comparingFamiltyTracker, DataProvider.GameModel, selectedHull, soundPlayer);
                    buttons.Add(button);
                    HasUnlockedItem = HasUnlockedItem || button.IsUnlocked;
                    itemContainer.gameObject.SetActive(button.IsUnlocked);
                }
                _button = buttons;
                return buttons;
            }
        }

        public IItemButton GetFirstItemButton()
        {
            return _button[0];
        }
    }
}
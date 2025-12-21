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
    public class ItemsPanel : Panel
    {
        private IList<ItemButton> _button;
        private ItemButton _lastSelectedButton;

        public ItemType itemType;
        public ItemType ItemType => itemType;

        public bool HasUnlockedItem { get; private set; }

        [SerializeField]
        private UnityEngine.UI.ScrollRect scrollRect;

        public HeckleItemContainerV2 HeckleItemContainerV2Prefab;
        public Transform heckleParent;
        private IList<ItemButton> buttons = new List<ItemButton>();
        private ItemDetailsManager _itemDetailsManager;
        private ComparingItemFamilyTracker _comparingFamiltyTracker;
        private IBroadcastingProperty<HullKey> _selectedHull;
        private SingleSoundPlayer _soundPlayer;
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
                ItemButton button = heckleContainer.Initialise(this, _itemDetailsManager, _comparingFamiltyTracker, DataProvider.GameModel, _selectedHull, _soundPlayer);
                buttons.Add(button);
                heckleContainer.gameObject.SetActive(true);
            }
        }

        public IList<ItemButton> Initialise(
            ItemDetailsManager itemDetailsManager,
            ComparingItemFamilyTracker comparingFamiltyTracker,
            IBroadcastingProperty<HullKey> selectedHull,
            SingleSoundPlayer soundPlayer)
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
                IList<ItemButton> buttons = new List<ItemButton>();

                HasUnlockedItem = false;

                foreach (ItemContainer itemContainer in itemContainers)
                {
                    ItemButton button = itemContainer.Initialise(this, itemDetailsManager, comparingFamiltyTracker, DataProvider.GameModel, selectedHull, soundPlayer);
                    buttons.Add(button);
                    HasUnlockedItem = HasUnlockedItem || button.IsUnlocked;
                    itemContainer.gameObject.SetActive(button.IsUnlocked);
                }
                _button = buttons;
                return buttons;
            }
        }

        public void RegisterSelection(ItemButton button)
        {
            if (button != null && button.IsUnlocked)
                _lastSelectedButton = button;
        }

        public ItemButton GetLastSelectedButton()
        {
            return _lastSelectedButton;
        }

        public ItemButton GetPreferredItemButton()
        {
            return _lastSelectedButton ?? GetFirstItemButton();
        }

        public ItemButton GetFirstItemButton()
        {
            return _button[0];
        }

        public IList<ItemButton> GetAllButtons()
        {
            return _button;
        }

        public void ScrollToButton(ItemButton targetButton)
        {
            // Try to find ScrollRect if not assigned
            if (scrollRect == null)
            {
                scrollRect = GetComponent<UnityEngine.UI.ScrollRect>();
                if (scrollRect == null)
                {
                    scrollRect = GetComponentInParent<UnityEngine.UI.ScrollRect>();
                }
                if (scrollRect == null)
                {
                    scrollRect = GetComponentInChildren<UnityEngine.UI.ScrollRect>();
                }
            }

            if (scrollRect == null || _button == null || !_button.Contains(targetButton))
                return;

            // Calculate the position of the target button within the content
            int buttonIndex = _button.IndexOf(targetButton);
            if (buttonIndex < 0) return;

            // Assuming horizontal layout, calculate the normalized scroll position
            // This centers the target button in the viewport
            float totalButtons = _button.Count;
            if (totalButtons <= 1) return;

            float normalizedPosition = buttonIndex / (totalButtons - 1f);

            // For horizontal scrolling, we set horizontalNormalizedPosition
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(normalizedPosition);
        }
    }

    public enum ItemType
    {
        // Cruisers
        Hull,
        // Buildings
        Factory, Defense, Offensive, Tactical, Ultra,
        // Units
        Ship, Aircraft,
        // Profile
        Profile, Heckle, Captain
    }

    public enum ItemFamily
    {
        Hulls, Buildings, Units, Profile, Heckles, Captains
    }
}
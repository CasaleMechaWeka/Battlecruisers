using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public abstract class ItemButton : Togglable, IPointerClickHandler, IItemButton
    {
        private Image _itemImage;
        private Text _itemName;

        protected IItemDetailsManager _itemDetailsManager;
        protected IComparingItemFamilyTracker _comparingFamiltyTracker;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public bool IsUnlocked => IsVisible;
        public abstract IComparableItem Item { get; }

        public Color Color
        {
            set
            {
                _itemImage.color = value;
                _itemName.color = value;
            }
        }

        public ItemFamily itemFamily;

        public virtual void Initialise(IItemDetailsManager itemDetailsManager, IComparingItemFamilyTracker comparingFamiltyTracker)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamiltyTracker);

            _itemDetailsManager = itemDetailsManager;
            _comparingFamiltyTracker = comparingFamiltyTracker;
            _itemImage = transform.FindNamedComponent<Image>("ItemImage");

            _itemName = GetComponentInChildren<Text>();
            Assert.IsNotNull(_itemName);

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _comparingFamiltyTracker.ComparingFamily.ValueChanged += _comparingFamiltyTracker_ValueChanged;
        }

        private void _comparingFamiltyTracker_ValueChanged(object sender, EventArgs e)
        {
            Enabled 
                = _comparingFamiltyTracker.ComparingFamily.Value == null
                    || itemFamily == _comparingFamiltyTracker.ComparingFamily.Value;
        }

        public abstract void OnPointerClick(PointerEventData eventData);
    }
}
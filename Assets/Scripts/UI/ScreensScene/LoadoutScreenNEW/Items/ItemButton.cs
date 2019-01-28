using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public abstract class ItemButton : Togglable, IPointerClickHandler
    {
        protected IItemDetailsManager _itemDetailsManager;
        protected IBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup { get { return _canvasGroup; } }

        public ItemFamily itemFamily;

        public virtual void Initialise(IItemDetailsManager itemDetailsManager, IBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            Helper.AssertIsNotNull(itemDetailsManager, itemFamilyToCompare);

            _itemDetailsManager = itemDetailsManager;
            _itemFamilyToCompare = itemFamilyToCompare;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _itemFamilyToCompare.ValueChanged += _itemFamilyToCompare_ValueChanged;
        }

        private void _itemFamilyToCompare_ValueChanged(object sender, EventArgs e)
        {
            Enabled 
                = _itemFamilyToCompare.Value == null
                    || itemFamily == _itemFamilyToCompare.Value;
        }

        // FELIX  Avoid duplicate code in child classes?  Complicates initialisation code if this class becomes generic :/  Worth it?
        public abstract void OnPointerClick(PointerEventData eventData);
    }
}
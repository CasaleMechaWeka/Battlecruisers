using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public abstract class ItemButton : Togglable, IPointerClickHandler
    {
        protected IItemDetailsManager _itemDetailsManager;
        protected IComparingItemFamilyTracker _comparingFamiltyTracker;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup { get { return _canvasGroup; } }

        public ItemFamily itemFamily;

        public virtual void Initialise(IItemDetailsManager itemDetailsManager, IComparingItemFamilyTracker comparingFamiltyTracker)
        {
            Helper.AssertIsNotNull(itemDetailsManager, comparingFamiltyTracker);

            _itemDetailsManager = itemDetailsManager;
            _comparingFamiltyTracker = comparingFamiltyTracker;

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
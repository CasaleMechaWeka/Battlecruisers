using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using System;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class HeckleButtonV2 : ItemButton
    {
        private RectTransform _selectedFeedback;
        public override IComparableItem Item => null;
        private IComparingItemFamilyTracker _itemFamilyTracker;
        private IHeckleData _heckleData;

        public override void ShowDetails()
        {
            //  _itemDetailsManager.ShowDetails(null);
            _itemDetailsManager.ShowDetails(_heckleData.StringKeyBase);
        }

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IHeckleData heckleData,
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingItemFamily)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingItemFamily);
            _selectedFeedback = transform.FindNamedComponent<RectTransform>("SelectedFeedback");
            _heckleData = heckleData;
            _itemFamilyTracker = comparingItemFamily;
            _itemFamilyTracker.ComparingFamily.ValueChanged += OnUnitListChange;
            _itemName.text = Mathf.Max(108, 217 * heckleData.Index).ToString().Substring(0, 3);
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _comparingFamiltyTracker.SetComparingFamily(itemFamily);
            if (_comparingFamiltyTracker.ComparingFamily.Value == itemFamily)
            {
                _itemDetailsManager.ShowDetails(_heckleData.StringKeyBase);
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
            else
            {
                //_itemDetailsManager.CompareWithSelectedItem(_unitPrefab.Buildable);
                //_comparingFamiltyTracker.SetComparingFamily(null);
            }
        }

        private void UpdateSelectedFeedback()
        {
            _selectedFeedback.gameObject.SetActive(true);
        }
        private void OnUnitListChange(object sender, EventArgs e)
        {
            UpdateSelectedFeedback();
        }
    }
}

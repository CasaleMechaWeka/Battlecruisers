using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System;
using UnityCommon.Properties;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class HullButton : ItemButton
    {
        private HullKey _hullKey;
        private IBroadcastingProperty<HullKey> _selectedHull;
        private RectTransform _selectedFeedback;

        private Cruiser _cruiserPrefab;
        public override IComparableItem Item => _cruiserPrefab;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IItemDetailsManager itemDetailsManager, 
            IComparingItemFamilyTracker comparingFamiltyTracker,
            HullKey hullKey,
            Cruiser cruiserPrefab,
            IBroadcastingProperty<HullKey> selectedHull)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingFamiltyTracker);

            Helper.AssertIsNotNull(hullKey, cruiserPrefab, selectedHull);

            _hullKey = hullKey;
            _cruiserPrefab = cruiserPrefab;
            _selectedHull = selectedHull;
            _selectedFeedback = transform.FindNamedComponent<RectTransform>("SelectedFeedback");

            _selectedHull.ValueChanged += _selectedHull_ValueChanged;

            UpdateSelectedFeedback();
        }

        private void _selectedHull_ValueChanged(object sender, EventArgs e)
        {
            UpdateSelectedFeedback();
        }

        private void UpdateSelectedFeedback()
        {
            _selectedFeedback.gameObject.SetActive(_hullKey.Equals(_selectedHull.Value));
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            if (_comparingFamiltyTracker.ComparingFamily.Value == null)
            {
                _itemDetailsManager.ShowDetails(_cruiserPrefab);
            }
            else
            {
                _itemDetailsManager.CompareWithSelectedItem(_cruiserPrefab);
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
        }
    }
}
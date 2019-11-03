using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
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

        private Cruiser _cruiser;
        public override IComparableItem Item => _cruiser;

        public void Initialise(
            ISoundPlayer soundPlayer,
            IItemDetailsManager itemDetailsManager, 
            IComparingItemFamilyTracker comparingFamiltyTracker,
            HullKey hullKey,
            IBroadcastingProperty<HullKey> selectedHull,
            // FELIX  Move to base class
            IPrefabFactory prefabFactory)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingFamiltyTracker);

            Helper.AssertIsNotNull(selectedHull, hullKey);

            _hullKey = hullKey;
            _selectedHull = selectedHull;
            _selectedFeedback = transform.FindNamedComponent<RectTransform>("SelectedFeedback");
            _cruiser = prefabFactory.GetCruiserPrefab(hullKey);

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
                _itemDetailsManager.ShowDetails(_cruiser);
            }
            else
            {
                _itemDetailsManager.CompareWithSelectedItem(_cruiser);
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
        }
    }
}
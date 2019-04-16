using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;
using UnityCommon.Properties;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class HullButton : ItemButton
    {
        private HullKey _hullKey;
        private IBroadcastingProperty<HullKey> _selectedHull;
        private Image _selectedFeedback;

        public Cruiser cruiser;
        public override IComparableItem Item => cruiser;

        public void Initialise(
            IItemDetailsManager itemDetailsManager, 
            IComparingItemFamilyTracker comparingFamiltyTracker,
            HullKey hullKey,
            IBroadcastingProperty<HullKey> selectedHull)
        {
            base.Initialise(itemDetailsManager, comparingFamiltyTracker);

            Helper.AssertIsNotNull(cruiser, selectedHull, hullKey);

            _hullKey = hullKey;
            _selectedHull = selectedHull;
            _selectedFeedback = transform.FindNamedComponent<Image>("SelectedFeedback");
            cruiser.StaticInitialise();

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

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_comparingFamiltyTracker.ComparingFamily.Value == null)
            {
                _itemDetailsManager.ShowDetails(cruiser);
            }
            else
            {
                _itemDetailsManager.CompareWithSelectedItem(cruiser);
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
        }
    }
}
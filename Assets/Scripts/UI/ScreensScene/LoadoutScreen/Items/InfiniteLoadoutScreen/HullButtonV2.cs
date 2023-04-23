using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using TMPro;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class HullButtonV2 : ItemButton
    {
        private HullKey _hullKey;
        private IBroadcastingProperty<HullKey> _selectedHull;
        private RectTransform _selectedFeedback;
        public TextMeshProUGUI _unitName;

        private Cruiser _cruiserPrefab;
        public override IComparableItem Item => _cruiserPrefab;
        public GameObject clickedFeedBack;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IItemDetailsManager itemDetailsManager, 
            IComparingItemFamilyTracker comparingFamiltyTracker,
            HullKey hullKey,
            Cruiser cruiserPrefab,
            IBroadcastingProperty<HullKey> selectedHull,
            PrefabKeyName hullKeyName)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingFamiltyTracker);

            Helper.AssertIsNotNull(hullKey, cruiserPrefab, selectedHull);

            _hullKey = hullKey;
            _cruiserPrefab = cruiserPrefab;
            _selectedHull = selectedHull;
            _selectedFeedback = transform.FindNamedComponent<RectTransform>("SelectedFeedback");

            _selectedHull.ValueChanged += _selectedHull_ValueChanged;
            _unitName.text = (hullKeyName.ToString()).Replace("Hull_", string.Empty);

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
            //clickedFeedBack.SetActive(true);

            _comparingFamiltyTracker.SetComparingFamily(itemFamily);
            if (_comparingFamiltyTracker.ComparingFamily.Value == ItemFamily.Hulls)
            {
                _itemDetailsManager.ShowDetails(_cruiserPrefab);
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
            else
            {
                //_itemDetailsManager.CompareWithSelectedItem(_cruiserPrefab);
                //_comparingFamiltyTracker.SetComparingFamily(null);
            }
        }

        public override void ShowDetails()
        {
            _itemDetailsManager.ShowDetails(_cruiserPrefab);
        }
    }
}
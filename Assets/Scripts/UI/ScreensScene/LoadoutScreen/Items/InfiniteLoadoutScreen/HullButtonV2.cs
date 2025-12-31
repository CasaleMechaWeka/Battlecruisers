using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Data.Static;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class HullButtonV2 : ItemButton
    {
        private HullKey _hullKey;
        private IBroadcastingProperty<HullKey> _selectedHull;
        private RectTransform _selectedFeedback;
        public Text _unitName;

        private Cruiser _cruiserPrefab;
        public override IComparableItem Item => _cruiserPrefab;
        public GameObject clickedFeedBack;
        public Button toggleHullButton;
        public SelectCruiserButton selectCruiserButton;

        public HullKey GetHullKey()
        {
            return _hullKey;
        }

        public void Initialise(
            SingleSoundPlayer soundPlayer,
            ItemDetailsManager itemDetailsManager,
            ComparingItemFamilyTracker comparingFamiltyTracker,
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
            _unitName.text = cruiserPrefab.Name.ToString();
            UpdateSelectedFeedback();
            toggleHullButton.onClick.AddListener(OnSelectionButtonClicked);
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
                _itemDetailsManager.ShowDetails(StaticPrefabKeys.Hulls.GetHullType(_hullKey));
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
            _itemDetailsManager.ShowDetails(StaticPrefabKeys.Hulls.GetHullType(_hullKey));
        }
        private void OnSelectionButtonClicked()
        {
            if (!GetComponentInChildren<ClickedFeedBack>(true).gameObject.activeInHierarchy)
                OnClicked();
            selectCruiserButton.OnClickedAction();

        }
    }
}
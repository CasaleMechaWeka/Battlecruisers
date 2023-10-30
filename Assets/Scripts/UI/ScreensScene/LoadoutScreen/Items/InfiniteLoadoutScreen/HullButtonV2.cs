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
using UnityEngine.Assertions;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Data;
using BattleCruisers.Utils.Fetchers;
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
        private const char SEPARATOR = '_';
        private string lootType;
        private string lootName;
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
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

            string keyNameStr = hullKeyName.ToString();

            string[] strAsArray = keyNameStr.Split(SEPARATOR);
            Assert.AreEqual(2, strAsArray.Length);

            lootType = strAsArray[0];
            lootName = strAsArray[1];

            _hullKey = hullKey;
            _cruiserPrefab = cruiserPrefab;
            _selectedHull = selectedHull;
            _selectedFeedback = transform.FindNamedComponent<RectTransform>("SelectedFeedback");

            _selectedHull.ValueChanged += _selectedHull_ValueChanged;
            _unitName.text = (cruiserPrefab.Name).ToString();
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
                _itemDetailsManager.ShowDetails(GetHullType(_hullKey));
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
            else
            {
                //_itemDetailsManager.CompareWithSelectedItem(_cruiserPrefab);
                //_comparingFamiltyTracker.SetComparingFamily(null);
            }
        }

        private HullType GetHullType(HullKey hullKey)
        {
            switch (hullKey.PrefabName)
            {
                case "Trident":
                    return HullType.Trident;
                case "BlackRig":
                    return HullType.BlackRig;
                case "Bullshark":
                    return HullType.Bullshark;
                case "Eagle":
                    return HullType.Eagle;
                case "Hammerhead":
                    return HullType.Hammerhead;
                case "Longbow":
                    return HullType.Longbow;
                case "Megalodon":
                    return HullType.Megalodon;
                case "Raptor":
                    return HullType.Raptor;
                case "Rickshaw":
                    return HullType.Rickshaw;
                case "Rockjaw":
                    return HullType.Rockjaw;
                case "TasDevil":
                    return HullType.TasDevil;
                default:
                    return HullType.Yeti;
            }
        }

        public override void ShowDetails()
        {
            _itemDetailsManager.ShowDetails(_cruiserPrefab);
            _itemDetailsManager.ShowDetails(GetHullType(_hullKey));
        }
        private void OnSelectionButtonClicked()
        {
            if (!GetComponentInChildren<ClickedFeedBack>(true).gameObject.activeInHierarchy)
                OnClicked();
            selectCruiserButton.OnClickedAction();

        }
    }
}
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Scenes;
using BattleCruisers.Data.Static;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class UnitButtonV2 : ItemButton
    {
        private IBuildableWrapper<IUnit> _unitPrefab;
        private IComparingItemFamilyTracker _itemFamilyTracker;
        private IGameModel _gameModel;
        private UnitKey _unitkey;
        public SelectUnitButton selectUnitButton;
        public override IComparableItem Item => _unitPrefab.Buildable;
        public Text _unitName;
        private RectTransform _selectedFeedback;
        public Button toggleSelectionButton;
        public Image variantIcon;
        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingItemFamily,
            IBuildableWrapper<IUnit> unitPrefab,
            PrefabKeyName unitKeyName,
            IGameModel gameModel,
            UnitKey key)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingItemFamily);

            //_unitName.text = (unitKeyName.ToString()).Replace("Unit_", string.Empty);
            _selectedFeedback = transform.FindNamedComponent<RectTransform>("SelectedFeedback");
            Assert.IsNotNull(_selectedFeedback);
            _itemFamilyTracker = comparingItemFamily;
            _gameModel = gameModel;
            _unitkey = key;
            _unitName.text = (unitPrefab.Buildable.Name).ToString();
            _itemFamilyTracker.ComparingFamily.ValueChanged += OnUnitListChange;
            Assert.IsNotNull(unitPrefab);
            _unitPrefab = unitPrefab;
            toggleSelectionButton.onClick.AddListener(OnSelectionToggleClicked);

            // show variant icon in item button when init load
            VariantPrefab variant = _gameModel.PlayerLoadout.GetSelectedUnitVariant(ScreensSceneGod.Instance._prefabFactory, _unitPrefab.Buildable);
            if (variant != null)
            {
                variantIcon.gameObject.SetActive(true);
                variantIcon.sprite = variant.variantSprite;
            }
            else
            {
                variantIcon.gameObject.SetActive(false);
            }
            variantChanged += OnVariantChanged;
        }

        private void OnVariantChanged(object sender, VariantChangeEventArgs args)
        {
            int index = args.Index;
            if (index != -1)
            {
                VariantPrefab variant = ScreensSceneGod.Instance._prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
                if (variant != null)
                {
                    variantIcon.gameObject.SetActive(true);
                    variantIcon.sprite = variant.variantSprite;
                }
                else
                {
                    variantIcon.gameObject.SetActive(false);
                }
            }
            else
            {
                variantIcon.gameObject.SetActive(false);
            }
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _comparingFamiltyTracker.SetComparingFamily(itemFamily);
            if (_comparingFamiltyTracker.ComparingFamily.Value == itemFamily)
            {
                // _itemDetailsManager.ShowDetails(_unitPrefab.Buildable);
                _itemDetailsManager.ShowDetails(_unitPrefab.Buildable, this);
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
            else
            {
                //_itemDetailsManager.CompareWithSelectedItem(_unitPrefab.Buildable);
                //_comparingFamiltyTracker.SetComparingFamily(null);
            }
        }

        public override void ShowDetails()
        {
            //         _itemDetailsManager.ShowDetails(_unitPrefab.Buildable);
            _itemDetailsManager.ShowDetails(_unitPrefab.Buildable, this);
        }

        private void UpdateSelectedFeedback()
        {
            _selectedFeedback.gameObject.SetActive(_gameModel.PlayerLoadout.IsUnitInList(_unitPrefab.Buildable.Category, _unitkey));
        }

        private void OnUnitListChange(object sender, EventArgs e)
        {
            UpdateSelectedFeedback();
        }

        private void OnSelectionToggleClicked()
        {
            Loadout loadout = _gameModel.PlayerLoadout;
            if (!GetComponentInChildren<ClickedFeedBack>(true).gameObject.activeInHierarchy)
                OnClicked();
            if (_gameModel.PlayerLoadout.GetUnitListSize(_unitPrefab.Buildable.Category) > 1 || !loadout.GetUnitKeys(_unitPrefab.Buildable.Category).Contains(_unitkey))
                selectUnitButton.ToggleUnitSelection();
        }
    }
}
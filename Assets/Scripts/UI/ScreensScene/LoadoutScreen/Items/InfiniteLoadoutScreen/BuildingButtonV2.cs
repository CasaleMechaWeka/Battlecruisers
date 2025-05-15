using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
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
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class BuildingButtonV2 : ItemButton
    {
        public IBuildableWrapper<IBuilding> _buildingPrefab;
        private ComparingItemFamilyTracker _itemFamilyTracker;
        private GameModel _gameModel;
        private BuildingKey _buildingKey;

        public SelectBuildingButton selectBuildingButton;
        public override IComparableItem Item => _buildingPrefab.Buildable;
        public Text _buildingName;
        private RectTransform _selectedFeedback;
        public Button toggleSelectionButton;
        public Image variantIcon;

        public void Initialise(
            SingleSoundPlayer soundPlayer,
            IItemDetailsManager itemDetailsManager,
            ComparingItemFamilyTracker comparingFamiltyTracker,
            IBuildableWrapper<IBuilding> buildingPrefab,
            GameModel gameModel,
            BuildingKey buildingKey)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingFamiltyTracker);
            //_buildingName.text = (buildingKeyName.ToString()).Replace("Building_", string.Empty);
            _selectedFeedback = transform.FindNamedComponent<RectTransform>("SelectedFeedback");
            Assert.IsNotNull(_selectedFeedback);
            _itemFamilyTracker = comparingFamiltyTracker;
            _itemFamilyTracker.ComparingFamily.ValueChanged += OnListChange;
            _gameModel = gameModel;
            _buildingKey = buildingKey;
            Assert.IsNotNull(buildingPrefab);
            _buildingPrefab = buildingPrefab;
            _buildingName.text = (buildingPrefab.Buildable.Name).ToString();

            toggleSelectionButton.onClick.AddListener(OnSelectionToggleClicked);

            UpdateSelectedFeedback();

            // show variant icon in item button when init load
            VariantPrefab variant = _gameModel.PlayerLoadout.GetSelectedBuildingVariant(_buildingPrefab.Buildable);
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
                VariantPrefab variant = PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
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

            _comparingFamiltyTracker.SetComparingFamily(ItemFamily.Buildings);
            if (_comparingFamiltyTracker.ComparingFamily.Value == ItemFamily.Buildings)
            {
                //    _itemDetailsManager.ShowDetails(_buildingPrefab.Buildable);
                _itemDetailsManager.ShowDetails(_buildingPrefab.Buildable, this);
                _comparingFamiltyTracker.SetComparingFamily(null);
            }
            else
            {
                //_itemDetailsManager.CompareWithSelectedItem(_buildingPrefab.Buildable);
                //_comparingFamiltyTracker.SetComparingFamily(null);
            }
        }

        public override void ShowDetails()
        {
            //    _itemDetailsManager.ShowDetails(_buildingPrefab.Buildable);
            _itemDetailsManager.ShowDetails(_buildingPrefab.Buildable, this);
        }

        private void UpdateSelectedFeedback()
        {
            _selectedFeedback.gameObject.SetActive(_gameModel.PlayerLoadout.IsBuildingInList(_buildingPrefab.Buildable.Category, _buildingKey));
        }

        private void OnListChange(object sender, EventArgs e)
        {
            UpdateSelectedFeedback();
        }

        private void OnSelectionToggleClicked()
        {
            if (!GetComponentInChildren<ClickedFeedBack>(true).gameObject.activeInHierarchy)
                OnClicked();
            selectBuildingButton.ToggleBuildingSelection();
        }
    }
    public class VariantChangeEventArgs : EventArgs
    {
        public int Index;
    }
}
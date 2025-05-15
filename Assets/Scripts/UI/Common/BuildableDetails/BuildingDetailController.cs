using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class BuildingDetailController : MonoBehaviour
    {
        private int _selectedVariant;
        public CanvasGroupButton leftNav, rightNav;
        private IBuilding _selectedBuilding;
        public IBuilding SelectedBuilding
        {
            get { return _selectedBuilding; }
            set
            {
                _selectedBuilding = value;
                SetInitVariant();
            }
        }

        private ItemButton _currentButton;
        public ItemButton CureentButton
        {
            get => _currentButton;
            set { _currentButton = value; }
        }

        private SingleSoundPlayer _soundPlayer;
        private Dictionary<IBuilding, List<int>> _unlockedVariants;
        private int _index;

        public Image variantIcon;
        public Text variantName;
        public Text variantDescription;
        public Text variantParentName;
        public StatsController<IBuilding> variantStats;

        public void Initialize(SingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(soundPlayer);
            Helper.AssertIsNotNull(leftNav, rightNav);
            _soundPlayer = soundPlayer;

            leftNav.Initialise(_soundPlayer, LeftNavButton_OnClicked);
            rightNav.Initialise(_soundPlayer, RightNavButton_OnClicked);

            variantIcon.gameObject.SetActive(false);
            variantName.gameObject.SetActive(false);

            CollectUnlockedBuildingVariant();
        }
        private void SetInitVariant()
        {
            _selectedVariant = DataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(_selectedBuilding);
            if (_unlockedVariants.ContainsKey(_selectedBuilding))
            {
                if (_unlockedVariants[_selectedBuilding].Count > 0)
                {
                    if (_unlockedVariants[_selectedBuilding].IndexOf(_selectedVariant) == 0)
                    {
                        if (_unlockedVariants[_selectedBuilding].Count == 1)
                        {
                            leftNav.gameObject.SetActive(true);
                            rightNav.gameObject.SetActive(false);
                        }
                        else
                        {
                            leftNav.gameObject.SetActive(true);
                            rightNav.gameObject.SetActive(true);
                        }
                        _index = 0;
                        ShowVariantDetail(_unlockedVariants[_selectedBuilding][_index]);
                        return;
                    }
                    else if (_unlockedVariants[_selectedBuilding].IndexOf(_selectedVariant) == _unlockedVariants[_selectedBuilding].Count - 1)
                    {
                        leftNav.gameObject.SetActive(true);
                        rightNav.gameObject.SetActive(false);
                        _index = _unlockedVariants[_selectedBuilding].Count - 1;
                        ShowVariantDetail(_unlockedVariants[_selectedBuilding][_index]);
                        return;
                    }
                    else if (_unlockedVariants[_selectedBuilding].IndexOf(_selectedVariant) < 0)
                    {
                        leftNav.gameObject.SetActive(false);
                        rightNav.gameObject.SetActive(true);
                        _index = -1;
                        ShowOriginalBuilding();
                        return;
                    }
                    else
                    {
                        leftNav.gameObject.SetActive(true);
                        rightNav.gameObject.SetActive(true);
                        _index = _unlockedVariants[_selectedBuilding].IndexOf(_selectedVariant);
                        ShowVariantDetail(_unlockedVariants[_selectedBuilding][_index]);
                        return;
                    }
                }
                else
                {
                    leftNav.gameObject.SetActive(false);
                    rightNav.gameObject.SetActive(false);
                    _index = -1;
                    ShowOriginalBuilding();
                }
            }
            else
            {
                leftNav.gameObject.SetActive(false);
                rightNav.gameObject.SetActive(false);
                _index = -1;
                ShowOriginalBuilding();
            }
            _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = _selectedVariant });
        }
        private void ShowVariantDetail(int index)
        {
            if (index < 0)
                return;
            variantIcon.gameObject.SetActive(true);
            variantName.gameObject.SetActive(true);
            VariantPrefab variant = PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
            variantName.text = LocTableCache.CommonTable.GetString(StaticData.Variants[index].VariantNameStringKeyBase);
            variantIcon.sprite = variant.variantSprite;
            variantParentName.text = variant.GetParentName();
            variantDescription.text = LocTableCache.CommonTable.GetString(StaticData.Variants[index].VariantDescriptionStringKeyBase);
            variantStats.ShowStatsOfVariant(_selectedBuilding, variant);
        }

        private void ShowOriginalBuilding()
        {
            variantIcon.gameObject.SetActive(false);
            variantName.gameObject.SetActive(false);
            GetComponent<ComparableBuildingDetailsController>().ShowItemDetails();
        }
        private void LeftNavButton_OnClicked()
        {
            --_index;
            int current_index = DataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(_selectedBuilding);
            if (_index <= -1)
            {
                _index = -1;
                leftNav.gameObject.SetActive(false);
                rightNav.gameObject.SetActive(true);
                ShowOriginalBuilding();
                DataProvider.GameModel.PlayerLoadout.RemoveCurrentSelectedVariant(current_index);
                DataProvider.SaveGame();
                _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = -1 });
                return;
            }
            else
            {
                leftNav.gameObject.SetActive(true);
                rightNav.gameObject.SetActive(true);
            }
            DataProvider.GameModel.PlayerLoadout.RemoveCurrentSelectedVariant(current_index);
            DataProvider.GameModel.PlayerLoadout.AddSelectedVariant(_unlockedVariants[_selectedBuilding][_index]);
            DataProvider.SaveGame();
            ShowVariantDetail(_unlockedVariants[_selectedBuilding][_index]);
            _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = _unlockedVariants[_selectedBuilding][_index] });
        }
        private void RightNavButton_OnClicked()
        {
            ++_index;
            int current_index = DataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(_selectedBuilding);
            if (_index >= _unlockedVariants[_selectedBuilding].Count - 1)
            {
                _index = _unlockedVariants[_selectedBuilding].Count - 1;
                leftNav.gameObject.SetActive(true);
                rightNav.gameObject.SetActive(false);
            }
            else
            {
                leftNav.gameObject.SetActive(true);
                rightNav.gameObject.SetActive(true);
            }
            DataProvider.GameModel.PlayerLoadout.RemoveCurrentSelectedVariant(current_index);
            DataProvider.GameModel.PlayerLoadout.AddSelectedVariant(_unlockedVariants[_selectedBuilding][_index]);
            DataProvider.SaveGame();
            ShowVariantDetail(_unlockedVariants[_selectedBuilding][_index]);
            _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = _unlockedVariants[_selectedBuilding][_index] });
        }

        public void CollectUnlockedBuildingVariant()
        {
            _unlockedVariants = new Dictionary<IBuilding, List<int>>();
            for (int i = 0; i < DataProvider.GameModel.PurchasedVariants.Count; i++)
            {
                VariantPrefab variant = PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(DataProvider.GameModel.PurchasedVariants[i]));
                if (variant != null)
                    if (!variant.IsUnit())
                        if (_unlockedVariants.ContainsKey(variant.GetBuilding()))
                            _unlockedVariants[variant.GetBuilding()].Add(DataProvider.GameModel.PurchasedVariants[i]);
                        else
                            _unlockedVariants[variant.GetBuilding()] = new List<int> { DataProvider.GameModel.PurchasedVariants[i] };
            }
        }
    }
}

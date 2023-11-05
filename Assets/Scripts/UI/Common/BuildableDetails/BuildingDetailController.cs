using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class BuildingDetailController : MonoBehaviour
    {
        private int _selectedVariant;
        private IBuilding _selectedBuilding;
        public CanvasGroupButton leftNav, rightNav;
        public IBuilding SelectedBuilding
        {
            get { return _selectedBuilding; }
            set
            {
                _selectedBuilding = value;
                SetInitVariant();
            }
        }


        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;
        private ILocTable _commonStrings;
        private Dictionary<IBuilding, List<int>> _unlockedVariants;
        private int _index;

        public void Initialize(IDataProvider dataProvider, IPrefabFactory prefabFactory, ISingleSoundPlayer soundPlayer, ILocTable commonString)
        {
            Helper.AssertIsNotNull(dataProvider, prefabFactory, soundPlayer);
            Helper.AssertIsNotNull(leftNav, rightNav);
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _soundPlayer = soundPlayer;
            _commonStrings = commonString;

            leftNav.Initialise(_soundPlayer, LeftNavButton_OnClicked);
            rightNav.Initialise(_soundPlayer, RightNavButton_OnClicked);
        }
        private async void SetInitVariant()
        {
            _selectedVariant = await _dataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(_prefabFactory, _selectedBuilding);
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
                        ShowVariantDetail();
                        return;
                    }
                    else if (_unlockedVariants[_selectedBuilding].IndexOf(_selectedVariant) == _unlockedVariants[_selectedBuilding].Count - 1)
                    {
                        leftNav.gameObject.SetActive(true);
                        rightNav.gameObject.SetActive(false);
                        _index = _unlockedVariants[_selectedBuilding].Count - 1;
                        ShowVariantDetail();
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
                        ShowVariantDetail();
                        return;
                    }
                }
                else
                {
                    leftNav.gameObject.SetActive(false);
                    rightNav.gameObject.SetActive(false);
                    _index = -1;
                }
            }
            else
            {
                leftNav.gameObject.SetActive(false);
                rightNav.gameObject.SetActive(false);
                _index = -1;
            }
        }
        private void ShowVariantDetail()
        {

        }

        private void ShowOriginalBuilding()
        {

        }
        private void LeftNavButton_OnClicked()
        {

        }
        private void RightNavButton_OnClicked()
        {

        }

        public async void CollectUnlockedBuildingVariant()
        {
            _unlockedVariants = new Dictionary<IBuilding, List<int>>();
            for (int i = 0; i < _dataProvider.GameModel.GetVariants().Count; i++)
            {
                VariantPrefab variant = await _prefabFactory.GetVariant(StaticPrefabKeys.Variants.AllKeys[_dataProvider.GameModel.GetVariants()[i]]);
                if (variant != null)
                {
                    if (!variant.IsUnit())
                    {
                        if (_unlockedVariants.ContainsKey(variant.GetBuilding()))
                        {
                            _unlockedVariants[variant.GetBuilding()].Add(_dataProvider.GameModel.GetVariants()[i]);
                        }
                        else
                        {
                            _unlockedVariants[variant.GetBuilding()] = new List<int> { _dataProvider.GameModel.GetVariants()[i] };
                        }
                    }
                }
            }
        }
    }
}

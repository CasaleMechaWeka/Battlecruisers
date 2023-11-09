using BattleCruisers.Buildables.Units;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
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
    public class UnitDetailController : MonoBehaviour
    {
        private int _selectedVariant;
        public CanvasGroupButton leftNav, rightNav;
        private IUnit _selectedUnit;
        public IUnit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                SetInitVariant();
            }
        }
        private ItemButton _currentButton;
        public ItemButton currentButton
        {
            get => _currentButton;
            set { _currentButton = value; }
        }

        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;
        private ILocTable _commonStrings;
        private Dictionary<IUnit, List<int>> _unlockedVariants;
        private int _index;

        public Image variantIcon;
        public Text variantName;
        public Text variantDescription;
        public Text variantParentName;
        public StatsController<IUnit> variantStats;

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
            CollectUnlockedUnitVariant();

        }

        private async void SetInitVariant()
        {
            _selectedVariant = await _dataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(_prefabFactory, _selectedUnit);
            if (_unlockedVariants.ContainsKey(_selectedUnit))
            {
                if (_unlockedVariants[_selectedUnit].Count > 0)
                {
                    if (_unlockedVariants[_selectedUnit].IndexOf(_selectedVariant) == 0)
                    {
                        if (_unlockedVariants[_selectedUnit].Count == 1)
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
                        ShowVariantDetail(_unlockedVariants[_selectedUnit][_index]);
                        return;
                    }
                    else if(_unlockedVariants[_selectedUnit].IndexOf(_selectedVariant) == _unlockedVariants[_selectedUnit].Count - 1)
                    {
                        leftNav.gameObject.SetActive(true);
                        rightNav.gameObject.SetActive(false);
                        _index = _unlockedVariants[_selectedUnit].Count - 1;
                        ShowVariantDetail(_unlockedVariants[_selectedUnit][_index]);
                        return;
                    }
                    else if (_unlockedVariants[_selectedUnit].IndexOf(_selectedVariant) < 0)
                    {
                        leftNav.gameObject.SetActive(false);
                        rightNav.gameObject.SetActive(true);
                        _index = -1;
                        ShowOriginalUnit();
                        return;
                    }
                    else
                    {
                        leftNav.gameObject.SetActive(true);
                        rightNav.gameObject.SetActive(true);
                        _index = _unlockedVariants[_selectedUnit].IndexOf(_selectedVariant);
                        ShowVariantDetail(_unlockedVariants[_selectedUnit][_index]);
                        return;
                    }
                }
                else
                {
                    leftNav.gameObject.SetActive(false);
                    rightNav.gameObject.SetActive(false);
                    _index = -1;
                    ShowOriginalUnit();
                }
            }
            else
            {
                leftNav.gameObject.SetActive(false);
                rightNav.gameObject.SetActive(false);
                _index = -1;
                ShowOriginalUnit();
            }
            _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = _selectedVariant });
        }
        private async void ShowVariantDetail(int index)
        {
            if (index < 0)
                return;
            variantIcon.gameObject.SetActive(true);
            variantName.gameObject.SetActive(true);
            VariantPrefab variant = await _prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(index));
            variantName.text = _commonStrings.GetString(_dataProvider.GameModel.Variants[index].VariantNameStringKeyBase);
            variantIcon.sprite = variant.variantSprite;
            variantParentName.text = variant.GetParentName(ScreensSceneGod.Instance._prefabFactory);
            variantDescription.text = _commonStrings.GetString(_dataProvider.GameModel.Variants[index].variantDescriptionStringKeyBase);
            variantStats.ShowStatsOfVariant(_selectedUnit, variant);
        }

        private void ShowOriginalUnit()
        {
            variantIcon.gameObject.SetActive(false);
            variantName.gameObject.SetActive(false);
            GetComponent<ComparableUnitDetailsController>().ShowItemDetails();
        }
        private async void LeftNavButton_OnClicked()
        {
            --_index;
            int current_index = await _dataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(_prefabFactory, _selectedUnit);
            if (_index <= -1)
            {
                _index = -1;
                leftNav.gameObject.SetActive(false);
                rightNav.gameObject.SetActive(true);
                ShowOriginalUnit();
                _dataProvider.GameModel.PlayerLoadout.RemoveCurrentSelectedVariant(current_index);
                _dataProvider.SaveGame();
                _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = -1 });
                return;
            }
            else
            {
                leftNav.gameObject.SetActive(true);
                rightNav.gameObject.SetActive(true);
            }
            _dataProvider.GameModel.PlayerLoadout.RemoveCurrentSelectedVariant(current_index);
            _dataProvider.GameModel.PlayerLoadout.AddSelectedVariant(_unlockedVariants[_selectedUnit][_index]);
            _dataProvider.SaveGame();
            ShowVariantDetail(_unlockedVariants[_selectedUnit][_index]);
            _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = _unlockedVariants[_selectedUnit][_index] });
        }
        private async void RightNavButton_OnClicked()
        {
            ++_index;
            int current_index = await _dataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(_prefabFactory, _selectedUnit);
            if (_index >= _unlockedVariants[_selectedUnit].Count - 1)
            {
                _index = _unlockedVariants[_selectedUnit].Count - 1;
                leftNav.gameObject.SetActive(true);
                rightNav.gameObject.SetActive(false);
            }
            else
            {
                leftNav.gameObject.SetActive(true);
                rightNav.gameObject.SetActive(true);
            }
            _dataProvider.GameModel.PlayerLoadout.RemoveCurrentSelectedVariant(current_index);
            _dataProvider.GameModel.PlayerLoadout.AddSelectedVariant(_unlockedVariants[_selectedUnit][_index]);
            _dataProvider.SaveGame();
            ShowVariantDetail(_unlockedVariants[_selectedUnit][_index]);
            _currentButton.variantChanged.Invoke(this, new VariantChangeEventArgs { Index = _unlockedVariants[_selectedUnit][_index] });
        }

        public async void CollectUnlockedUnitVariant()
        {
            _unlockedVariants = new Dictionary<IUnit, List<int>>();
            for (int i = 0; i < _dataProvider.GameModel.GetVariants().Count; i++)
            {
                VariantPrefab variant = await _prefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(_dataProvider.GameModel.GetVariants()[i]));
                if (variant != null)
                {
                    if (variant.IsUnit())
                    {
                        if (_unlockedVariants.ContainsKey(variant.GetUnit(ScreensSceneGod.Instance._prefabFactory)))
                        {
                            _unlockedVariants[variant.GetUnit(ScreensSceneGod.Instance._prefabFactory)].Add(_dataProvider.GameModel.GetVariants()[i]);
                        }
                        else
                        {
                            _unlockedVariants[variant.GetUnit(ScreensSceneGod.Instance._prefabFactory)] = new List<int> { _dataProvider.GameModel.GetVariants()[i] };
                        }
                    }
                }
            }
        }
    }
}

using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class UnitButtonV2 : ItemButton
    {
        private IBuildableWrapper<IUnit> _unitPrefab;
        private IComparingItemFamilyTracker _itemFamilyTracker;
        private IGameModel _gameModel;
        private UnitKey _unitkey;
        public override IComparableItem Item => _unitPrefab.Buildable;
        public TextMeshProUGUI _unitName;
        private RectTransform _selectedFeedback;

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
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            _comparingFamiltyTracker.SetComparingFamily(itemFamily);
            if (_comparingFamiltyTracker.ComparingFamily.Value == itemFamily)
            {
                _itemDetailsManager.ShowDetails(_unitPrefab.Buildable);
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
            _itemDetailsManager.ShowDetails(_unitPrefab.Buildable);
        }

        private void UpdateSelectedFeedback()
        {
            _selectedFeedback.gameObject.SetActive(_gameModel.PlayerLoadout.IsUnitInList(_unitPrefab.Buildable.Category, _unitkey));
        }

        private void OnUnitListChange(object sender, EventArgs e)
        {
            UpdateSelectedFeedback();
        }
    }
}
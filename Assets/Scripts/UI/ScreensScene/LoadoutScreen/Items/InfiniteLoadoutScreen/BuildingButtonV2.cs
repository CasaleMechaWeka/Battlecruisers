using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class BuildingButtonV2 : ItemButton
    {
        public IBuildableWrapper<IBuilding> _buildingPrefab;
        private IComparingItemFamilyTracker _itemFamilyTracker;
        private IGameModel _gameModel;
        private BuildingKey _buildingKey;
        public override IComparableItem Item => _buildingPrefab.Buildable;
        public TextMeshProUGUI _buildingName;
        private RectTransform _selectedFeedback;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingFamiltyTracker,
            IBuildableWrapper<IBuilding> buildingPrefab,
            IGameModel gameModel,
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

            UpdateSelectedFeedback();
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _comparingFamiltyTracker.SetComparingFamily(ItemFamily.Buildings);
            if (_comparingFamiltyTracker.ComparingFamily.Value == ItemFamily.Buildings)
            {
                _itemDetailsManager.ShowDetails(_buildingPrefab.Buildable);
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
            _itemDetailsManager.ShowDetails(_buildingPrefab.Buildable);
        }

        private void UpdateSelectedFeedback()
        {
            _selectedFeedback.gameObject.SetActive(_gameModel.PlayerLoadout.IsBuildingInList(_buildingPrefab.Buildable.Category, _buildingKey));
        }

        private void OnListChange(object sender, EventArgs e)
        {
            UpdateSelectedFeedback();
        }
    }
}
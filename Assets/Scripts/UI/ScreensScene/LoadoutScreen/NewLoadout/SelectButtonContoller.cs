using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;
using UnityEngine;
using System.Collections.Generic;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using TMPro;
using Unity.Services.Analytics;
using BattleCruisers.Buildables.Buildings;
using UnityEditorInternal;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class SelectButtonContoller : CanvasGroupButton
    {
        private IItemDetailsDisplayer<IBuilding> _buildingDetails;
        private IItemDetailsDisplayer<IUnit> _unitDetails;
        private IBuildingNameToKey _buildingNameToKey;
        private IDataProvider _dataProvider;
        private IBroadcastingProperty<ItemFamily?> _comparingFamily;

        public TextMeshProUGUI limit;
        public GameObject selectText;
        public GameObject deselectText;

        private bool _selected;
        private bool flag;

        protected override bool ToggleVisibility => true;

        public void Initialise(ISingleSoundPlayer soundPlayer,
            IDataProvider dataProvider,
            IItemDetailsDisplayer<IBuilding> buildingDetails,
            IBuildingNameToKey buildingName,
            IBroadcastingProperty<ItemFamily?> _itemFamily)
        {
            base.Initialise(soundPlayer);
            Helper.AssertIsNotNull(dataProvider, buildingDetails, buildingName, _itemFamily);

            _dataProvider = dataProvider;
            _buildingDetails = buildingDetails;
            _comparingFamily = _itemFamily;
            _comparingFamily.ValueChanged += UpdateSelectBuildingButton;
            _buildingDetails.SelectedItem.ValueChanged += DisplayChange;
            _buildingNameToKey = buildingName;

            UpdateSelectText();
            Enabled = ShouldBeEnabled();
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            IBuilding displayBuilding = _buildingDetails.SelectedItem.Value;
            Assert.IsNotNull(displayBuilding);
            BuildingKey buildingKey = _buildingNameToKey.GetKey(displayBuilding.Name);

            Loadout playerLoadout = _dataProvider.GameModel.PlayerLoadout;
            List<BuildingKey> buildingKeys = playerLoadout.GetBuildingKeys(displayBuilding.Category);
            Assert.IsNotNull(buildingKeys);
            if(!buildingKeys.Contains(buildingKey))
            {
                playerLoadout.AddbuildItem(displayBuilding.Category, buildingKey);
                limit.text = playerLoadout.GetBuildingListSize(displayBuilding.Category).ToString();
                _selected = true;
            }
            UpdateSelectText();
        }

        private void UpdateSelectText()
        {
            if (selectText.activeSelf)
            {
                selectText.SetActive(false);
                deselectText.SetActive(true);
            }
            else 
            {
                selectText.SetActive(true);
                deselectText.SetActive(false);
            }
        }
        
        private bool ShouldBeEnabled()
        {
            
            Debug.Log("it is comparing");
            Debug.Log(_comparingFamily.Value);
            if (_comparingFamily.Value == ItemFamily.Buildings)
                flag = true;
            else if (_comparingFamily.Value == ItemFamily.Units || _comparingFamily.Value == ItemFamily.Hulls)
                flag = false;
            else if (_comparingFamily.Value == null)
            {
                //do nothing
            }
               
            return flag;
        }

        private void UpdateSelectBuildingButton(object sender, EventArgs e)
        {
            Enabled = ShouldBeEnabled();
        }
        private void DisplayChange(object sender, EventArgs e)
        {
            Enabled = ShouldBeEnabled();
            UpdateSelectText();
        }
    }
}


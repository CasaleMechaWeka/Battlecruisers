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
    public class SelectBuildingButton : CanvasGroupButton
    {
        private IItemDetailsDisplayer<IBuilding> _buildingDetails;
        private IBuildingNameToKey _buildingNameToKey;
        private IDataProvider _dataProvider;
        private IBroadcastingProperty<ItemFamily?> _comparingFamily;
        private IComparingItemFamilyTracker _comparingItemFamilyTracker;

        public TextMeshProUGUI limit;
        public GameObject selectText;
        public GameObject deselectText;
        public int buildingLimit;
        public GameObject checkBox;

        private bool flag;

        protected override bool ToggleVisibility => true;

        public void Initialise(ISingleSoundPlayer soundPlayer,
            IDataProvider dataProvider,
            IItemDetailsDisplayer<IBuilding> buildingDetails,
            IBuildingNameToKey buildingName,
            IBroadcastingProperty<ItemFamily?> _itemFamily,
            IComparingItemFamilyTracker comparingItemFamily)
        {
            base.Initialise(soundPlayer);
            Helper.AssertIsNotNull(dataProvider, buildingDetails, buildingName, _itemFamily);

            _dataProvider = dataProvider;
            _buildingDetails = buildingDetails;
            _comparingItemFamilyTracker = comparingItemFamily;
            _comparingFamily = _itemFamily;
            _comparingFamily.ValueChanged += UpdateSelectBuildingButton;
            _buildingDetails.SelectedItem.ValueChanged += DisplayedBuildingChanged;
            _buildingNameToKey = buildingName;

            UpdateSelectText(true);
            Enabled = IsOverLimit();
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
                if(playerLoadout.GetBuildingListSize(displayBuilding.Category) <= buildingLimit)
                    playerLoadout.AddbuildItem(displayBuilding.Category, buildingKey);
                _dataProvider.SaveGame();
                limit.text = playerLoadout.GetBuildingListSize(displayBuilding.Category).ToString();
                UpdateSelectText(true);
            }
            else
            {
                playerLoadout.RemoveBuildItem(displayBuilding.Category, buildingKey);
                _dataProvider.SaveGame();
                limit.text = playerLoadout.GetBuildingListSize(displayBuilding.Category).ToString();
                UpdateSelectText(false);
            }
            _comparingItemFamilyTracker.SetComparingFamily(ItemFamily.Buildings);
            _comparingItemFamilyTracker.SetComparingFamily(null);
        }

        private void UpdateSelectText(bool value)
        {
            if (value)
            {
                checkBox.SetActive(true);
                Enabled = IsOverLimit();
                selectText.SetActive(false);
                deselectText.SetActive(true);
            }
            else 
            {
                checkBox.SetActive(false);
                Enabled = IsOverLimit();
                selectText.SetActive(true);
                deselectText.SetActive(false);
            }
        }
        
        private bool ShouldBeEnabled()
        {
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
            Enabled = IsOverLimit();
        }
        private void DisplayedBuildingChanged(object sender, EventArgs e)
        {
            Debug.Log("it is checking");
            IBuilding displayBuilding = _buildingDetails.SelectedItem.Value;
            Debug.Log(displayBuilding);
            Loadout playerLoadout = _dataProvider.GameModel.PlayerLoadout;
            
            //Assert.IsNotNull(displayBuilding);
            if (displayBuilding != null)
            {
                BuildingKey buildingKey = _buildingNameToKey.GetKey(displayBuilding.Name);
                limit.text = playerLoadout.GetBuildingListSize(displayBuilding.Category).ToString();

                List<BuildingKey> buildingKeys = playerLoadout.GetBuildingKeys(displayBuilding.Category);
                Assert.IsNotNull(buildingKeys);
                if (buildingKeys.Contains(buildingKey))
                    UpdateSelectText(true);
                else
                    UpdateSelectText(false);
            }
        }

        private bool IsOverLimit()
        {
            Loadout loadout = _dataProvider.GameModel.PlayerLoadout;
            if(_buildingDetails != null)
            {
                BuildingKey key = _buildingNameToKey.GetKey(_buildingDetails.SelectedItem.Value.Name);
                if (ShouldBeEnabled() && (loadout.GetBuildingListSize(_buildingDetails.SelectedItem.Value.Category) <= buildingLimit))
                    if(loadout.IsBuildingInList(_buildingDetails.SelectedItem.Value.Category, key))
                    {
                        return true;
                    }
            }    
            return false;
        }
    }
}


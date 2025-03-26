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
using TMPro;
using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class SelectBuildingButton : CanvasGroupButton
    {
        private IItemDetailsDisplayer<IBuilding> _buildingDetails;
        private IBuildingNameToKey _buildingNameToKey;
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
            IItemDetailsDisplayer<IBuilding> buildingDetails,
            IBuildingNameToKey buildingName,
            IBroadcastingProperty<ItemFamily?> _itemFamily,
            IComparingItemFamilyTracker comparingItemFamily)
        {
            base.Initialise(soundPlayer);
            Helper.AssertIsNotNull(buildingDetails, buildingName, _itemFamily);

            _buildingDetails = buildingDetails;
            _comparingItemFamilyTracker = comparingItemFamily;
            _comparingFamily = _itemFamily;
            _comparingFamily.ValueChanged += UpdateSelectBuildingButton;
            _buildingDetails.SelectedItem.ValueChanged += DisplayedBuildingChanged;
            _buildingNameToKey = buildingName;

            UpdateSelectText(true);
            Enabled = ShouldBeEnabled();
        }

        public void ToggleBuildingSelection() { OnClicked(); }

        protected override void OnClicked()
        {
            base.OnClicked();

            IBuilding displayBuilding = _buildingDetails.SelectedItem.Value;
            Assert.IsNotNull(displayBuilding);
            BuildingKey buildingKey = _buildingNameToKey.GetKey(displayBuilding.Name);

            Loadout playerLoadout = DataProvider.GameModel.PlayerLoadout;
            List<BuildingKey> buildingKeys = playerLoadout.GetBuildingKeys(displayBuilding.Category);
            Assert.IsNotNull(buildingKeys);
            if (!buildingKeys.Contains(buildingKey))
            {
                if (playerLoadout.GetBuildingListSize(displayBuilding.Category) < buildingLimit)
                {
                    playerLoadout.AddBuildingItem(displayBuilding.Category, buildingKey);
                    UpdateSelectText(true);
                }
                DataProvider.SaveGame();
                limit.text = playerLoadout.GetBuildingListSize(displayBuilding.Category).ToString();
            }
            else
            {
                playerLoadout.RemoveBuildItem(displayBuilding.Category, buildingKey);
                DataProvider.SaveGame();
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
            else if (_comparingFamily.Value == ItemFamily.Units || _comparingFamily.Value == ItemFamily.Hulls || _comparingFamily.Value == ItemFamily.Heckles)
                flag = false;
            else if (_comparingFamily.Value == null)
            {
                //do nothing
            }

            return flag;
        }

        private void UpdateSelectBuildingButton(object sender, EventArgs e)
        {
            if (ShouldBeEnabled())
            {
                Enabled = IsOverLimit();
            }
            else
            {
                Enabled = false;
            }
        }
        private void DisplayedBuildingChanged(object sender, EventArgs e)
        {
            IBuilding displayBuilding = _buildingDetails.SelectedItem.Value;
            Loadout playerLoadout = DataProvider.GameModel.PlayerLoadout;

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
                {
                    UpdateSelectText(false);
                }
            }
        }

        private bool IsOverLimit()
        {
            Loadout loadout = DataProvider.GameModel.PlayerLoadout;
            if (_buildingDetails.SelectedItem.Value != null)
            {
                if ((loadout.GetBuildingListSize(_buildingDetails.SelectedItem.Value.Category) == buildingLimit) && selectText.activeSelf)
                {
                    return false;
                }
            }
            return true;
        }
    }
}


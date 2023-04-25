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
    public class SelectUnitButton : CanvasGroupButton
    {
        private IItemDetailsDisplayer<IUnit> _unitDetails;
        private IUnitNameToKey _unitNameToKey;
        private IDataProvider _dataProvider;
        private IBroadcastingProperty<ItemFamily?> _comparingFamily;
        private IComparingItemFamilyTracker _comparingItemFamilyTracker;

        public TextMeshProUGUI limit;
        public GameObject selectText;
        public GameObject deselectText;
        public int unitLimit;
        public GameObject checkBox;

        private bool flag;

        protected override bool ToggleVisibility => true;

        public void Initialise(ISingleSoundPlayer soundPlayer,
            IDataProvider dataProvider,
            IItemDetailsDisplayer<IUnit> unitDetails,
            IUnitNameToKey unitName,
            IBroadcastingProperty<ItemFamily?> _itemFamily,
            IComparingItemFamilyTracker comparingItemFamily)
        {
            base.Initialise(soundPlayer);
            Helper.AssertIsNotNull(dataProvider, unitDetails, unitName, _itemFamily);

            _dataProvider = dataProvider;
            _unitDetails = unitDetails;
            _comparingItemFamilyTracker = comparingItemFamily;
            _comparingFamily = _itemFamily;
            _comparingFamily.ValueChanged += UpdateSelectUnitButton;
            _unitDetails.SelectedItem.ValueChanged += DisplayedUnitChanged;
            _unitNameToKey = unitName;

            UpdateSelectText(true);
            Enabled = IsOverLimit();
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            IUnit displayUnit = _unitDetails.SelectedItem.Value;
            Assert.IsNotNull(displayUnit);
            UnitKey unitKey = _unitNameToKey.GetKey(displayUnit.Name);

            Loadout playerLoadout = _dataProvider.GameModel.PlayerLoadout;
            List<UnitKey> unitKeys = playerLoadout.GetUnitKeys(displayUnit.Category);
            Assert.IsNotNull(unitKeys);
            if(!unitKeys.Contains(unitKey))
            {
                if(playerLoadout.GetUnitListSize(displayUnit.Category) <= unitLimit)
                    playerLoadout.AddUnitItem(displayUnit.Category, unitKey);
                _dataProvider.SaveGame();
                limit.text = playerLoadout.GetUnitListSize(displayUnit.Category).ToString();
                UpdateSelectText(true);
            }
            else
            {
                playerLoadout.RemoveUnitItem(displayUnit.Category, unitKey);
                _dataProvider.SaveGame();
                limit.text = playerLoadout.GetUnitListSize(displayUnit.Category).ToString();
                UpdateSelectText(false);
            }
            _comparingItemFamilyTracker.SetComparingFamily(ItemFamily.Units);
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
            if (_comparingFamily.Value == ItemFamily.Units)
                flag = true;
            else if (_comparingFamily.Value == ItemFamily.Buildings || _comparingFamily.Value == ItemFamily.Hulls)
                flag = false;
            else if (_comparingFamily.Value == null)
            {
                //do nothing
            }
               
            return flag;
        }

        private void UpdateSelectUnitButton(object sender, EventArgs e)
        {
            Enabled = ShouldBeEnabled();
        }
        private void DisplayedUnitChanged(object sender, EventArgs e)
        {
            Debug.Log("it is checking");
            IUnit displayUnit = _unitDetails.SelectedItem.Value;
            Debug.Log(displayUnit);
            //Assert.IsNotNull(displayBuilding);
            if (displayUnit != null)
            {
                UnitKey unitKey = _unitNameToKey.GetKey(displayUnit.Name);

                Loadout playerLoadout = _dataProvider.GameModel.PlayerLoadout;
                limit.text = playerLoadout.GetUnitListSize(displayUnit.Category).ToString();
                List<UnitKey> unitKeys = playerLoadout.GetUnitKeys(displayUnit.Category);
                Assert.IsNotNull(unitKeys);
                if (unitKeys.Contains(unitKey))
                    UpdateSelectText(true);
                else
                    UpdateSelectText(false);
            }
        }

        private bool IsOverLimit()
        {
            Loadout loadout = _dataProvider.GameModel.PlayerLoadout;
            if(_unitDetails != null)
                if (ShouldBeEnabled() && loadout.GetUnitListSize(_unitDetails.SelectedItem.Value.Category) <= unitLimit)
                    return true;
            return false;
        }
    }
}


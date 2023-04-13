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

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class SelectButtonContoller : CanvasGroupButton
    {
        private readonly IItemDetailsManager _itemDetailsManager;
        private IItemDetailsDisplayer<IBuildable> _buildingDetails;
        private IItemDetailsDisplayer<IUnit> _unitDetails;

        private IDataProvider _dataProvider;
        private RectTransform _selectedText;
        private RectTransform _deselectedText;
        private List<BuildableKey> _buildList;
        private List<UnitKey> _unitList;
        public ItemPanelsController itemPanelsController;

        public TextMeshProUGUI limit;
        public GameObject selectText;
        public GameObject deselectText;

        protected override bool ToggleVisibility => true;

        public void Initialise(ISingleSoundPlayer soundPlayer,
            IDataProvider dataProvider)
        {
            base.Initialise(soundPlayer);
            Helper.AssertIsNotNull(dataProvider);

            _dataProvider = dataProvider;
            //_selectedText = transform.FindNamedComponent<RectTransform>("SelectText");
            //_deselectedText = transform.FindNamedComponent<RectTransform>("DeselectText");
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            ItemType itemType = GetItem();
            if(itemType == ItemType.Hull)
            {
                //nothing
            } 
            else if (itemType == ItemType.Aircraft || itemType == ItemType.Ship)
            {
                AddUnit(itemType);
                
            }
            else
            {
               Addbuild(itemType);
            }
        }

        private ItemType GetItem()
        {
            IItemsPanel itemsPanel = itemPanelsController.CurrentlyShownPanel;
            ItemType type = itemsPanel.ItemType;
            return type;
        }

        private void Addbuild(ItemType itemType)
        {
            List<BuildingKey> buildList = _dataProvider.GameModel.PlayerLoadout.GetBuildingKeys(itemType);
            Assert.IsNotNull(buildList);
            if (_dataProvider.GameModel.PlayerLoadout.GetBuildingListSize(itemType) <= 5)
                _dataProvider.GameModel.PlayerLoadout.AddbuildItem(itemType, (BuildingKey)_itemDetailsManager.SelectedItem.Value);
            limit.text = _dataProvider.GameModel.PlayerLoadout.GetBuildingListSize(itemType).ToString();
            selectText.SetActive(false);
            deselectText.SetActive(true);
        }

        private void AddUnit(ItemType itemType)
        {
            List<UnitKey> unitList = _dataProvider.GameModel.PlayerLoadout.GetUnitKeys(itemType);
            Assert.IsNotNull(unitList);
            if (_dataProvider.GameModel.PlayerLoadout.GetUnitListSize(itemType) <= 4)
                _dataProvider.GameModel.PlayerLoadout.AddUnitItem(itemType, (UnitKey)_itemDetailsManager.SelectedItem.Value);
            limit.text = _dataProvider.GameModel.PlayerLoadout.GetBuildingListSize(itemType).ToString();
            selectText.SetActive(false);
            deselectText.SetActive(true);
        }
    }
}


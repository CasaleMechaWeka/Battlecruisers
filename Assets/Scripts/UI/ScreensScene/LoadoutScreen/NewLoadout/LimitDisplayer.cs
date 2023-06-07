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
using static BattleCruisers.Effects.Smoke.StaticSmokeStats;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class LimitDisplayer : MonoBehaviour
    {
        private IDataProvider _dataProvider;
        private IItemDetailsDisplayer<IBuilding> _displayer;
        private IItemDetailsDisplayer<IUnit> _unit;
        private IComparingItemFamilyTracker _familyTracker;
        private IBuildingNameToKey _buildingName;
        private IUnitNameToKey _unitName;

        public GameObject _limitText;
        private TextMeshPro _displayText;

        public void Initialise(IDataProvider dataProvider,
            IItemDetailsDisplayer<IBuilding> buildingDetails,
            IItemDetailsDisplayer<IUnit> unitDetails,
            IComparingItemFamilyTracker comparingItemFamily)
            //IBuildingNameToKey buildingNameToKey,
            //IUnitNameToKey unitNameToKey)
        {
            _dataProvider = dataProvider;
            _displayer = buildingDetails;
            _unit = unitDetails;
            _familyTracker = comparingItemFamily;
            //_buildingName = buildingNameToKey;
            //_unitName = unitNameToKey;
            _displayText = _limitText.GetComponent<TextMeshPro>();

            _familyTracker.ComparingFamily.ValueChanged += UpdateLimitDisplayer;
            this.gameObject.SetActive(false);
        }

        private void UpdateLimitDisplayer(object sender, EventArgs e)
        {
            if(_familyTracker.ComparingFamily.Value == ItemFamily.Buildings)
            {
                this.gameObject.SetActive(true);

            }
            else if(_familyTracker.ComparingFamily.Value == ItemFamily.Units)
            {
                this.gameObject.SetActive(true);
            }
            else if(_familyTracker.ComparingFamily.Value == ItemFamily.Hulls)
            {
                this.gameObject.SetActive(false);
            }
            else if(_familyTracker.ComparingFamily.Value == null)
            {
                //do nothing
            }
        }
    }
}


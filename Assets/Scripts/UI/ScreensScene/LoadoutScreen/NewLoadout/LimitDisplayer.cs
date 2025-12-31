using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using System;
using UnityEngine;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Buildables.Units;
using TMPro;
using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class LimitDisplayer : MonoBehaviour
    {
        private ItemDetailsDisplayer<IBuilding> _displayer;
        private ItemDetailsDisplayer<IUnit> _unit;
        private ProfileDetailsController _profile;
        private ComparingItemFamilyTracker _familyTracker;
        private IBuildingNameToKey _buildingName;
        private IUnitNameToKey _unitName;

        public GameObject _limitText;
        public GameObject allowedLimitText;
        private TextMeshPro _displayText;
        public int BuildableLimit = 5, HeckleLimit = 3;

        public void Initialise(
            ItemDetailsDisplayer<IBuilding> buildingDetails,
            ItemDetailsDisplayer<IUnit> unitDetails,
            ProfileDetailsController profileDetails,
            ComparingItemFamilyTracker comparingItemFamily)
        //IBuildingNameToKey buildingNameToKey,
        //IUnitNameToKey unitNameToKey)
        {
            _displayer = buildingDetails;
            _unit = unitDetails;
            _profile = profileDetails;
            _familyTracker = comparingItemFamily;
            //_buildingName = buildingNameToKey;
            //_unitName = unitNameToKey;

            _familyTracker.ComparingFamily.ValueChanged += UpdateLimitDisplayer;
            this.gameObject.SetActive(false);
        }

        private void UpdateLimitDisplayer(object sender, EventArgs e)
        {
            TextMeshProUGUI displayText = allowedLimitText.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI limitText = _limitText.GetComponent<TextMeshProUGUI>();
            Loadout loadout = DataProvider.GameModel.PlayerLoadout;
            if (_familyTracker.ComparingFamily.Value == ItemFamily.Buildings)
            {
                gameObject.SetActive(true);
                displayText.text = BuildableLimit.ToString();
            }
            else if (_familyTracker.ComparingFamily.Value == ItemFamily.Units)
            {
                gameObject.SetActive(true);
                displayText.text = BuildableLimit.ToString();
            }
            else if (_familyTracker.ComparingFamily.Value == ItemFamily.Hulls)
            {
                gameObject.SetActive(false);
            }
            else if (_familyTracker.ComparingFamily.Value == ItemFamily.Profile)
            {
                gameObject.SetActive(false);
            }
            else if (_familyTracker.ComparingFamily.Value == null)
            {
                //do nothing
            }
        }
    }
}


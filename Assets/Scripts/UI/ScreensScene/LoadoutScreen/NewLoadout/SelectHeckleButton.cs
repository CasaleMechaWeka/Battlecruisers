using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Properties;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using BattleCruisers.Utils;
using System;
using BattleCruisers.Data.Models;
using UnityEngine.Assertions;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.ShopScreen;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class SelectHeckleButton : CanvasGroupButton
    {
        public TextMeshProUGUI limit;
        public GameObject selectText;
        public GameObject deselectText;
        public int heckleLimit;
        public GameObject checkBox;
        private IDataProvider _dataProvider;
        private HeckleDetailsController _heckleDetails;

        private bool flag;

        protected override bool ToggleVisibility => true;
        private IBroadcastingProperty<ItemFamily?> _comparingFamily;

        public void Initialise(ISingleSoundPlayer soundPlayer,
            IDataProvider dataProvider,
            HeckleDetailsController heckleDetails,
            IBroadcastingProperty<ItemFamily?> _itemFamily,
            IComparingItemFamilyTracker comparingItemFamily)
        {
            base.Initialise(soundPlayer);
            Helper.AssertIsNotNull(dataProvider, heckleDetails, _itemFamily);
            _comparingFamily = _itemFamily;
            _heckleDetails = heckleDetails;
            _comparingFamily.ValueChanged += UpdateSelectHeckleButton;
            _heckleDetails.SelectedItem.ValueChanged += DisplayedUnitChanged;
            _dataProvider = dataProvider;
            UpdateSelectText(true);
            Enabled = ShouldBeEnabled();
        }


        protected override void OnClicked()
        {
            base.OnClicked();


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
            if (_comparingFamily.Value == ItemFamily.Heckles)
                flag = true;
            else if (_comparingFamily.Value == ItemFamily.Buildings || _comparingFamily.Value == ItemFamily.Hulls || _comparingFamily.Value == ItemFamily.Units)
                flag = false;
            else if (_comparingFamily.Value == null)
            {
                //do nothing
            }
            return flag;
        }

        private void UpdateSelectHeckleButton(object sender, EventArgs e)
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


        private void DisplayedUnitChanged(object sender, EventArgs e)
        {
            IHeckleData heckleData = _heckleDetails.SelectedItem.Value;
            if(heckleData != null)
            {
                Loadout playerLoadout = _dataProvider.GameModel.PlayerLoadout;
                limit.text = playerLoadout.CurrentHeckles.Count.ToString();

                if (playerLoadout.CurrentHeckles.Contains(heckleData.Index))
                    UpdateSelectText(true);
                else
                    UpdateSelectText(false);
            }
        }

        private bool IsOverLimit()
        {
            Loadout loadout = _dataProvider.GameModel.PlayerLoadout;

            return true;
        }
    }
}

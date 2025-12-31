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

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class SelectCruiserButton : CanvasGroupButton
    {
        private ItemDetailsDisplayer<ICruiser> _cruiserDetails;
        private ComparisonStateTracker _comparisonStateTracker;
        private IHullNameToKey _hullNameToKey;
        protected override bool ToggleVisibility => true;

        private ISettableBroadcastingProperty<HullKey> _selectedHull;
        public IBroadcastingProperty<HullKey> SelectedHull { get; private set; }

        public void Initialise(
            SingleSoundPlayer soundPlayer,
            ItemDetailsDisplayer<ICruiser> cruiserDetails,
            ComparisonStateTracker comparisonStateTracker,
            IHullNameToKey hullNameToKey)
        {
            base.Initialise(soundPlayer);
            Helper.AssertIsNotNull(cruiserDetails, comparisonStateTracker, hullNameToKey);

            _cruiserDetails = cruiserDetails;
            _cruiserDetails.SelectedItem.ValueChanged += SelectedCruiserChanged;

            _comparisonStateTracker = comparisonStateTracker;
            _comparisonStateTracker.State.ValueChanged += ComparisonStateChanged;

            _hullNameToKey = hullNameToKey;

            _selectedHull = new SettableBroadcastingProperty<HullKey>(initialValue: DataProvider.GameModel.PlayerLoadout.Hull);
            SelectedHull = new BroadcastingProperty<HullKey>(_selectedHull);

            Enabled = ShouldBeEnabled();
        }

        private void SelectedCruiserChanged(object sender, EventArgs e)
        {
            Enabled = ShouldBeEnabled();
        }

        private void ComparisonStateChanged(object sender, EventArgs e)
        {
            Enabled = ShouldBeEnabled();
        }

        public void OnClickedAction() { OnClicked(); }

        protected override void OnClicked()
        {
            base.OnClicked();

            ICruiser displayedCruiser = _cruiserDetails.SelectedItem.Value;
            Assert.IsNotNull(displayedCruiser);
            _selectedHull.Value = _hullNameToKey.GetKey(displayedCruiser.Name);
            Loadout playerLoadout = DataProvider.GameModel.PlayerLoadout;
            if (!playerLoadout.Hull.Equals(_selectedHull.Value))
            {
                playerLoadout.Hull = _selectedHull.Value;
                DataProvider.SaveGame();
            }
            Enabled = ShouldBeEnabled();
        }

        private bool ShouldBeEnabled()
        {
            return
               _comparisonStateTracker.State.Value == ComparisonState.NotComparing
               && _cruiserDetails.SelectedItem.Value != null
               && !IsDisplayedCruiserSelected(_cruiserDetails.SelectedItem.Value);
        }

        private bool IsDisplayedCruiserSelected(ICruiser displayedCruiser)
        {
            return _hullNameToKey.GetKey(displayedCruiser.Name) == SelectedHull.Value;
        }
    }
}
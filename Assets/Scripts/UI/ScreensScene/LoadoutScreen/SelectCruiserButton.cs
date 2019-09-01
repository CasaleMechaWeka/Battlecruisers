using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System;
using UnityCommon.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class SelectCruiserButton : CanvasGroupButton
    {
        private IItemDetailsDisplayer<ICruiser> _cruiserDetails;
        private IComparisonStateTracker _comparisonStateTracker;
        private IHullNameToKey _hullNameToKey;
        private IDataProvider _dataProvider;

        protected override bool ToggleVisibility => true;

        private ISettableBroadcastingProperty<HullKey> _selectedHull;
        public IBroadcastingProperty<HullKey> SelectedHull { get; private set; }

        public void Initialise(
            ISoundPlayer soundPlayer,
            IItemDetailsDisplayer<ICruiser> cruiserDetails, 
            IComparisonStateTracker comparisonStateTracker,
            IHullNameToKey hullNameToKey,
            IDataProvider dataProvider)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(cruiserDetails, comparisonStateTracker, hullNameToKey, dataProvider);

            _cruiserDetails = cruiserDetails;
            _cruiserDetails.SelectedItem.ValueChanged += SelectedCruiserChanged;

            _comparisonStateTracker = comparisonStateTracker;
            _comparisonStateTracker.State.ValueChanged += ComparisonStateChanged;

            _hullNameToKey = hullNameToKey;
            _dataProvider = dataProvider;

            _selectedHull = new SettableBroadcastingProperty<HullKey>(initialValue: dataProvider.GameModel.PlayerLoadout.Hull);
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

        protected override void OnClicked()
        {
            base.OnClicked();

            ICruiser displayedCruiser = _cruiserDetails.SelectedItem.Value;
            Assert.IsNotNull(displayedCruiser);
            _selectedHull.Value = _hullNameToKey.GetKey(displayedCruiser.Name);

            ILoadout playerLoadout = _dataProvider.GameModel.PlayerLoadout;

            if (!playerLoadout.Hull.Equals(_selectedHull.Value))
            {
                playerLoadout.Hull = _selectedHull.Value;
                _dataProvider.SaveGame();
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
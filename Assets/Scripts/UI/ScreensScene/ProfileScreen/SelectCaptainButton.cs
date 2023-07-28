using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class SelectCaptainButton : CanvasGroupButton
    {
        private IComparisonStateTracker _comparisonStateTracker;
        private IDataProvider _dataProvider;

        private ISettableBroadcastingProperty<CaptainExoKey> _selectedCaptain;
        public IBroadcastingProperty<CaptainExoKey> SelectedCaptain { get; private set; }
        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IComparisonStateTracker comparisonStateTracker,
            IDataProvider dataProvider
            )
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(comparisonStateTracker, dataProvider);

            _comparisonStateTracker = comparisonStateTracker;
            //_comparisonStateTracker.State.ValueChanged += ComparisonStateChanged;

            _dataProvider = dataProvider;
            _selectedCaptain = new SettableBroadcastingProperty<CaptainExoKey>(initialValue: dataProvider.GameModel.CurrentCaptain);

            SelectedCaptain = new BroadcastingProperty<CaptainExoKey>(_selectedCaptain);
        }

        private void SelectedCaptainChanged()
        {

        }

        protected override void OnClicked()
        {
            Debug.Log("Captain Selected...");
        }
        //private void ComparisonStateChanged(object sender, EventArgs e)
        //{
        //    Enabled = ShouldBeEnabled();
        //}
    }
}


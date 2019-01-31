using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    // FELIX  Create interface for Save button to consume :)
    public class SelectCruiserButton : Togglable, IPointerClickHandler
    {
        private IItemDetailsDisplayer<ICruiser> _cruiserDetails;
        private IComparisonStateTracker _comparisonStateTracker;

        protected override bool ToggleVisibility { get { return true; } }

        // FELIX
        //public IBroadcastingProperty<>

        public void Initialise(IItemDetailsDisplayer<ICruiser> cruiserDetails, IComparisonStateTracker comparisonStateTracker)
        {
            Helper.AssertIsNotNull(cruiserDetails, comparisonStateTracker);

            _cruiserDetails = cruiserDetails;
            _cruiserDetails.SelectedItem.ValueChanged += SelectedCruiserChanged;

            _comparisonStateTracker = comparisonStateTracker;
            _comparisonStateTracker.State.ValueChanged += ComparisonStateChanged;
        }

        private void SelectedCruiserChanged(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void ComparisonStateChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public class SaveButton : Togglable, IPointerClickHandler
    {
        private IDataProvider _dataProvider;
        private IBroadcastingProperty<HullKey> _selectedHull;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup { get { return _canvasGroup; } }

        public void Initialise(IDataProvider dataProvider, IBroadcastingProperty<HullKey> selectedHull)
        {
            base.Initialise();

            Helper.AssertIsNotNull(dataProvider, selectedHull);

            _dataProvider = dataProvider;
            _selectedHull = selectedHull;

            _selectedHull.ValueChanged += _selectedHull_ValueChanged;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            Enabled = ShouldBeEnabled();
        }

        private void _selectedHull_ValueChanged(object sender, EventArgs e)
        {
            Enabled = ShouldBeEnabled();
        }

        private bool ShouldBeEnabled()
        {
            return !_dataProvider.GameModel.PlayerLoadout.Hull.Equals(_selectedHull.Value);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ILoadout playerLoadout = _dataProvider.GameModel.PlayerLoadout;

            if (!playerLoadout.Hull.Equals(_selectedHull.Value))
            {
                playerLoadout.Hull = _selectedHull.Value;
                _dataProvider.SaveGame();
            }

            Enabled = ShouldBeEnabled();
        }
    }
}
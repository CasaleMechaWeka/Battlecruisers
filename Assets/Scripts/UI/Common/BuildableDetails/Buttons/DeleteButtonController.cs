using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class DeleteButtonController : CanvasGroupButton
    {
        private IUIManager _uiManager;
        private IFilter<ITarget> _buttonVisibilityFilter;
        private ILongPressIdentifier _longPressIdentifier;

        public float lightUpIntervalS = 0.25f;
        public Image activeImage;
        public List<Sprite> activeStateImages;

        private const int NUMBER_OF_ACTIVE_STATES = 3;

        private IBuildable _buildable;
        public IBuildable Buildable
        {
            private get { return _buildable; }
            set
            {
                _buildable = value;
                gameObject.SetActive(_buttonVisibilityFilter.IsMatch(_buildable));
            }
        }

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IUIManager uiManager,
            IFilter<ITarget> buttonVisibilityFilter,
            IUpdater updater)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(uiManager, buttonVisibilityFilter, updater);
            Assert.IsNotNull(activeImage);
            Assert.AreEqual(NUMBER_OF_ACTIVE_STATES, activeStateImages.Count);

            _uiManager = uiManager;
            _buttonVisibilityFilter = buttonVisibilityFilter;
            _longPressIdentifier = new LongPressIdentifier(this, TimeBC.Instance, updater, lightUpIntervalS);

            _longPressIdentifier.LongPressStart += _longPressIdentifier_LongPressStart;
            _longPressIdentifier.LongPressEnd += _longPressIdentifier_LongPressEnd;
            _longPressIdentifier.LongPressInterval += _longPressIdentifier_LongPressInterval;
        }

        private void _longPressIdentifier_LongPressStart(object sender, EventArgs e)
        {
            activeImage.sprite = activeStateImages[0];
            activeImage.gameObject.SetActive(true);

            if (Buildable.BuildableState == BuildableState.NotStarted)
            {
                Buildable.Destroy();
            }
        }

        private void _longPressIdentifier_LongPressEnd(object sender, EventArgs e)
        {
            activeImage.gameObject.SetActive(false);
        }

        private void _longPressIdentifier_LongPressInterval(object sender, EventArgs e)
        {
            if (_longPressIdentifier.IntervalNumber >= NUMBER_OF_ACTIVE_STATES)
            {
                OnLongPressComplete();
            }
            else
            {
                activeImage.sprite = activeStateImages[_longPressIdentifier.IntervalNumber];
            }
        }

        private void OnLongPressComplete()
        {
            _uiManager.HideItemDetails();
            Buildable.Destroy();
        }
    }
}
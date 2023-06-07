using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    public class PvPDeleteButtonController : PvPCanvasGroupButton
    {
        private IPvPUIManager _uiManager;
        private IPvPFilter<IPvPTarget> _buttonVisibilityFilter;
        private IPvPLongPressIdentifier _longPressIdentifier;

        public float lightUpIntervalS = 0.25f;
        public Image activeImage;
        public List<Sprite> activeStateImages;
        protected override IPvPSoundKey ClickSound => PvPSoundKeys.UI.Delete;

        private const int NUMBER_OF_ACTIVE_STATES = 3;

        private IPvPBuildable _buildable;
        public IPvPBuildable Buildable
        {
            private get { return _buildable; }
            set
            {
                _buildable = value;
                gameObject.SetActive(_buttonVisibilityFilter.IsMatch(_buildable));
            }
        }

        public void Initialise(
            IPvPSingleSoundPlayer soundPlayer,
            IPvPUIManager uiManager,
            IPvPFilter<IPvPTarget> buttonVisibilityFilter,
            IPvPUpdater updater
            )
        {
            base.Initialise(soundPlayer);

            PvPHelper.AssertIsNotNull(uiManager, buttonVisibilityFilter, updater);
            Assert.IsNotNull(activeImage);
            Assert.AreEqual(NUMBER_OF_ACTIVE_STATES, activeStateImages.Count);

            _uiManager = uiManager;
            _buttonVisibilityFilter = buttonVisibilityFilter;
            _longPressIdentifier = new PvPLongPressIdentifier(this, PvPTimeBC.Instance, updater, lightUpIntervalS);

            _longPressIdentifier.LongPressStart += _longPressIdentifier_LongPressStart;
            _longPressIdentifier.LongPressEnd += _longPressIdentifier_LongPressEnd;
            _longPressIdentifier.LongPressInterval += _longPressIdentifier_LongPressInterval;
        }

        private void _longPressIdentifier_LongPressStart(object sender, EventArgs e)
        {
            activeImage.sprite = activeStateImages[0];
            activeImage.gameObject.SetActive(true);
            _soundPlayer.PlaySoundAsync(ClickSound);
            if (Buildable.BuildableState == PvPBuildableState.NotStarted)
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
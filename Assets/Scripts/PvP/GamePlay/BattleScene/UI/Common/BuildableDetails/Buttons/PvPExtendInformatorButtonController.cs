using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound.Players;
using System;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    // FELIX  Hide for tutorial?
    public class PvPExtendInformatorButtonController : PvPCanvasGroupButton, IButton
    {
        private IPvPSlidingPanel _informatorPanel;

        public Image activeFeedback;
        public Image defaultImage;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IPvPSlidingPanel informatorPanel)
        {
            base.Initialise(soundPlayer);

            Assert.IsNotNull(activeFeedback);
            Assert.IsNotNull(defaultImage);
            Assert.IsNotNull(informatorPanel);

            _informatorPanel = informatorPanel;
            _informatorPanel.State.ValueChanged += State_ValueChanged;
        }

        private void State_ValueChanged(object sender, EventArgs e)
        {
            SetActiveFeedback(_informatorPanel.State.Value == PvPPanelState.Shown);
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            if (_informatorPanel.TargetState == PvPPanelState.Hidden)
            {
                _informatorPanel.Show();
                SetActiveFeedback(true);
            }
            else
            {
                _informatorPanel.Hide();
                SetActiveFeedback(false);
            }
        }

        private void SetActiveFeedback(bool isActive)
        {
            activeFeedback.gameObject.SetActive(isActive);
            defaultImage.gameObject.SetActive(!isActive);
        }
    }
}
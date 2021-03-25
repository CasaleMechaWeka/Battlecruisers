using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    // FELIX  Hide for tutorial?
    public class ExtendInformatorButtonController : CanvasGroupButton, IButton
    {
        private ISlidingPanel _informatorPanel;

        public Image activeFeedback;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            ISlidingPanel informatorPanel)
        {
            base.Initialise(soundPlayer);

            Assert.IsNotNull(activeFeedback);
            Assert.IsNotNull(informatorPanel);
            _informatorPanel = informatorPanel;
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            if (_informatorPanel.TargetState == PanelState.Hidden)
            {
                _informatorPanel.Show();
                activeFeedback.gameObject.SetActive(true);
            }
            else
            {
                _informatorPanel.Hide();
                activeFeedback.gameObject.SetActive(false);
            }
        }
    }
}
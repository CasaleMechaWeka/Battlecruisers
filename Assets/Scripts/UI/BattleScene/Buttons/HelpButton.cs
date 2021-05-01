using BattleCruisers.UI.BattleScene.HelpLabels;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    // FELIX  Will be split into 2 different buttons (off state, normal canvas AND on state, paused help label canvas)
    public class HelpButton : CanvasGroupButton
    {
        private Image _leverOff, _leverOn;
        private IHelpLabelManager _helpLabelManager;

        public void Initialise(ISingleSoundPlayer soundPlayer, IHelpLabelManager helpLabelManager)
        {
            base.Initialise(soundPlayer);

            Assert.IsNotNull(helpLabelManager);
            _helpLabelManager = helpLabelManager;

            _leverOff = transform.FindNamedComponent<Image>("LeverOff");
            _leverOn = transform.FindNamedComponent<Image>("LeverOn");

            UpdateLeverVisibility();

            _helpLabelManager.IsShown.ValueChanged += IsShown_ValueChanged;
        }

        private void IsShown_ValueChanged(object sender, EventArgs e)
        {
            UpdateLeverVisibility();
        }

        private void UpdateLeverVisibility()
        {
            _leverOff.enabled = !_helpLabelManager.IsShown.Value;
            _leverOn.enabled = _helpLabelManager.IsShown.Value;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            Logging.Log(Tags.HELP_LABELS, $"_helpLabelManager.IsShown.Value: {_helpLabelManager.IsShown.Value}");

            if (_helpLabelManager.IsShown.Value)
            {
                _helpLabelManager.HideHelpLabels();
            }
            else
            {
                _helpLabelManager.ShowHelpLables();
            }
        }
    }
}
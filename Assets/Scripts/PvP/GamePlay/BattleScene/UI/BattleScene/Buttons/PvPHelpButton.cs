using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.HelpLabels;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public class PvPHelpButton : PvPCanvasGroupButton
    {
        //private Image _leverOff, _leverOn;
        private IPvPHelpLabelManager _helpLabelManager;

        public void Initialise(IPvPSingleSoundPlayer soundPlayer, IPvPHelpLabelManager helpLabelManager)
        {
            base.Initialise(soundPlayer);

            Assert.IsNotNull(helpLabelManager);
            _helpLabelManager = helpLabelManager;

            //_leverOff = transform.FindNamedComponent<Image>("LeverOff");
            //_leverOn = transform.FindNamedComponent<Image>("LeverOn");

            UpdateLeverVisibility();

            _helpLabelManager.IsShown.ValueChanged += IsShown_ValueChanged;
        }

        private void IsShown_ValueChanged(object sender, EventArgs e)
        {
            UpdateLeverVisibility();
        }

        private void UpdateLeverVisibility()
        {
            //_leverOff.enabled = !_helpLabelManager.IsShown.Value;
            //_leverOn.enabled = _helpLabelManager.IsShown.Value;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            // Logging.Log(Tags.HELP_LABELS, $"_helpLabelManager.IsShown.Value: {_helpLabelManager.IsShown.Value}");

            if (_helpLabelManager.IsShown.Value)
            {
                _helpLabelManager.HideHelpLabels();
            }
            else
            {
                _helpLabelManager.ShowHelpLabels();
            }
        }
    }
}
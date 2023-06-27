using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using System;
using System.Diagnostics;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    public class PvPToggleDroneButtonController : PvPCanvasGroupButton, IPvPButton
    {
        private IPvPBuildable _buildable;
        public IPvPBuildable Buildable
        {
            private get { return _buildable; }
            set
            {

                if (_buildable != null)
                {
                    _buildable.ToggleDroneConsumerFocusCommand.CanExecuteChanged -= ToggleDroneConsumerFocusCommand_CanExecuteChanged;
                }

                _buildable = value;

                if (_buildable != null)
                {
                    _buildable.ToggleDroneConsumerFocusCommand.CanExecuteChanged += ToggleDroneConsumerFocusCommand_CanExecuteChanged;
                    //  UpdateVisibility();
                }
                UpdateVisibility();
            }
        }

        // Should only be visible for player buildables, not AI buildables
        private bool ShowToggleDroneButton
        {
            get
            {
                return
                   _buildable != null && _buildable.Faction == PvPFaction.Blues
                    && _buildable.ToggleDroneConsumerFocusCommand.CanExecute;
            }
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _buildable?.ToggleDroneConsumerFocusCommand.Execute();
        }

        private void ToggleDroneConsumerFocusCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            gameObject.SetActive(ShowToggleDroneButton);
        }
    }
}

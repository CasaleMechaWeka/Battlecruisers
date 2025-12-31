using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.UI;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    [RequireComponent(typeof(Image))]
    public class PvPToggleDroneButtonController : PvPCanvasGroupButton, IButton
    {
        private IPvPBuildable _buildable;
        private Image image;
        [SerializeField]
        private Sprite unfocusedSprite;
        [SerializeField]
        private Sprite focusedSprite;
        void Start()
        {
            image = GetComponent<Image>();
            Assert.IsNotNull(image);
            Assert.IsNotNull(unfocusedSprite, "Sprite for unfocusedSprite was not assigned");
            Assert.IsNotNull(focusedSprite, "Sprite for unfocusedSprite was not assigned");
        }
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
                   _buildable != null && (SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT ? _buildable.Faction == Faction.Blues : _buildable.Faction == Faction.Reds)
                    && _buildable.ToggleDroneConsumerFocusCommand.CanExecute;
            }
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _buildable?.ToggleDroneConsumerFocusCommand.ExecuteIfPossible();
        }

        private void ToggleDroneConsumerFocusCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }
        private void OnBuildableDroneNumChange(object sender, BattleCruisers.Cruisers.Drones.DroneNumChangedEventArgs e)
        {
            UpdateVisibility();
        }

        private void UpdateSprite()
        {
            if (_buildable == null || _buildable.DroneConsumer == null)
                return;

            Debug.Log(_buildable.DroneConsumer.NumOfDrones);
            if (_buildable.DroneConsumer.NumOfDrones == _buildable.ParentCruiser.DroneManager.NumOfDrones)
                image.sprite = focusedSprite;
            else
                image.sprite = unfocusedSprite;
        }

        private void UpdateVisibility()
        {
            gameObject.SetActive(ShowToggleDroneButton);
            UpdateSprite();
        }
    }
}

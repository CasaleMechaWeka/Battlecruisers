using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    [RequireComponent(typeof(Image))]
    public class PvPToggleDroneButtonController : PvPCanvasGroupButton, IPvPButton
    {
        private IPvPBuildable _buildable;
        private Image image;
        [SerializeField]
        private Sprite unfocusedSprite;
        [SerializeField]
        private Sprite focusedSprite;
        void Start()
        {
            Assert.IsNotNull(image = GetComponent<Image>());
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
                   _buildable != null && (SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT ? _buildable.Faction == PvPFaction.Blues : _buildable.Faction == PvPFaction.Reds)
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
        private void OnBuildableDroneNumChange(object sender, BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.PvPDroneNumChangedEventArgs e)
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

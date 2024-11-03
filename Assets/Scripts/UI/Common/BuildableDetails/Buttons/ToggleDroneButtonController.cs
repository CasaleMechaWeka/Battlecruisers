using BattleCruisers.Buildables;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    [RequireComponent(typeof(Image))]
    public class ToggleDroneButtonController : CanvasGroupButton, IButton
    {
        private IBuildable _buildable;
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
        public IBuildable Buildable
        {
            private get { return _buildable; }
            set
            {
                if (_buildable != null)
                {
                    _buildable.ToggleDroneConsumerFocusCommand.CanExecuteChanged -= ToggleDroneConsumerFocusCommand_CanExecuteChanged;
                    _buildable.DroneNumChanged -= OnBuildableDroneNumChange;
                }

                _buildable = value;

                if (_buildable != null)
                {
                    _buildable.ToggleDroneConsumerFocusCommand.CanExecuteChanged += ToggleDroneConsumerFocusCommand_CanExecuteChanged;
                    _buildable.DroneNumChanged += OnBuildableDroneNumChange;
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
                  _buildable != null && _buildable.Faction == Faction.Blues
                    && _buildable.ToggleDroneConsumerFocusCommand.CanExecute;
            }
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _buildable?.ToggleDroneConsumerFocusCommand.Execute();
            UpdateSprite();
        }

        private void ToggleDroneConsumerFocusCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        private void OnBuildableDroneNumChange(object sender, Cruisers.Drones.DroneNumChangedEventArgs e)
        {
            UpdateVisibility();
        }
        private void UpdateSprite()
        {
            if (_buildable == null || _buildable.DroneConsumer == null)
                return;

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

using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers;
using System;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class ChooseTargetButtonController : UIElement, IButton
    {
        private Button _button;
        private IUserChosenTargetManager _userChosenTargetManager;

        private ITarget _target;
        public ITarget Target
        {
            private get { return _target; }
            set
            {
                _target = value;
				UpdateVisibility();
            }
        }

        // Should only be visible for AI buildings or cruiser
        private bool ShowButton 
        { 
            get 
            {
                return
                    _target != null
                    && _target.Faction == Faction.Reds
                    && (_target.TargetType == TargetType.Buildings
                        || _target.TargetType == TargetType.Cruiser);
            } 
        }

        public event EventHandler Clicked;

        public void Initialise(IUserChosenTargetManager userChosenTargetManager)
        {
            base.Initialise();

            Assert.IsNotNull(userChosenTargetManager);
            _userChosenTargetManager = userChosenTargetManager;

            _button = GetComponent<Button>();
            Assert.IsNotNull(_button);
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            ITarget currentUserChosenTarget = _userChosenTargetManager.HighestPriorityTarget != null ? _userChosenTargetManager.HighestPriorityTarget.Target : null;

            if (ReferenceEquals(currentUserChosenTarget, _target))
            {
                // Clear user chosen target
                _userChosenTargetManager.Target = null;
            }
            else
            {
                // Set user chosen target
                _userChosenTargetManager.Target = _target;
            }
   
            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }

        private void UpdateVisibility()
        {
            gameObject.SetActive(ShowButton);
        }
    }
}

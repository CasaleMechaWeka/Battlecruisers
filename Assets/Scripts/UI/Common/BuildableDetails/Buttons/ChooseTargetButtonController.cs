using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class ChooseTargetButtonController : UIElement, IButton
    {
        private Button _button;
        private IUserChosenTargetHelper _userChosenTargetHelper;
        private IFilter<ITarget> _buttonVisibilityFilter;

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

        private bool ShowButton { get { return _buttonVisibilityFilter.IsMatch(Target); } }

        public event EventHandler Clicked;

        public void Initialise(IUserChosenTargetHelper userChosenTargetHelper, IFilter<ITarget> buttonVisibilityFilter)
        {
            base.Initialise();

            Helper.AssertIsNotNull(userChosenTargetHelper, buttonVisibilityFilter);

            _userChosenTargetHelper = userChosenTargetHelper;
            _buttonVisibilityFilter = buttonVisibilityFilter;

            _button = GetComponent<Button>();
            Assert.IsNotNull(_button);
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _userChosenTargetHelper.ToggleChosenTarget(_target);
   
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

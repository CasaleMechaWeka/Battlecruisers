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
        private Text _buttonText;
        private IUserChosenTargetHelper _userChosenTargetHelper;
        private IFilter<ITarget> _buttonVisibilityFilter;

        private const string TARGET = "Target";
        private const string UNTARGET = "Untarget";

        private ITarget _target;
        public ITarget Target
        {
            private get { return _target; }
            set
            {
                _target = value;

                gameObject.SetActive(ShowButton);
                UpdateButtonText();
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

            _userChosenTargetHelper.UserChosenTargetChanged += (sender, e) => UpdateButtonText();

            _button = GetComponent<Button>();
            Assert.IsNotNull(_button);
            _button.onClick.AddListener(OnClick);

            _buttonText = GetComponentInChildren<Text>();
            Assert.IsNotNull(_buttonText);
        }

        private void OnClick()
        {
            _userChosenTargetHelper.ToggleChosenTarget(_target);

            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }

        private void UpdateButtonText()
        {
            _buttonText.text = ReferenceEquals(_target, _userChosenTargetHelper.UserChosenTarget) ? UNTARGET : TARGET;
        }
    }
}

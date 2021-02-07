using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class ChooseTargetButtonController : CanvasGroupButton, IButton
    {
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

        private bool ShowButton => _buttonVisibilityFilter.IsMatch(Target);

        public void Initialise(ISingleSoundPlayer soundPlayer, IUserChosenTargetHelper userChosenTargetHelper, IFilter<ITarget> buttonVisibilityFilter)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(userChosenTargetHelper, buttonVisibilityFilter);

            _userChosenTargetHelper = userChosenTargetHelper;
            _buttonVisibilityFilter = buttonVisibilityFilter;

            _userChosenTargetHelper.UserChosenTargetChanged += (sender, e) => UpdateButtonText();

            _buttonText = GetComponentInChildren<Text>();
            Assert.IsNotNull(_buttonText);
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _userChosenTargetHelper.ToggleChosenTarget(_target);
        }

        private void UpdateButtonText()
        {
            _buttonText.text = ReferenceEquals(_target, _userChosenTargetHelper.UserChosenTarget) ? UNTARGET : TARGET;
        }
    }
}

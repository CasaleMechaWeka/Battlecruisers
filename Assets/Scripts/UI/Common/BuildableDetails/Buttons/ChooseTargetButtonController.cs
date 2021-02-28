using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class ChooseTargetButtonController : CanvasGroupButton, IButton
    {
        private Text _buttonText;
        private IUserChosenTargetHelper _userChosenTargetHelper;
        private IFilter<ITarget> _buttonVisibilityFilter;
        private string _targetText, _untargetText;

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

        public void Initialise(
            ISingleSoundPlayer soundPlayer, 
            IUserChosenTargetHelper userChosenTargetHelper, 
            IFilter<ITarget> buttonVisibilityFilter,
            ILocTable commonStrings)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(userChosenTargetHelper, buttonVisibilityFilter, commonStrings);

            _userChosenTargetHelper = userChosenTargetHelper;
            _buttonVisibilityFilter = buttonVisibilityFilter;

            _userChosenTargetHelper.UserChosenTargetChanged += (sender, e) => UpdateButtonText();

            _buttonText = GetComponentInChildren<Text>();
            Assert.IsNotNull(_buttonText);

            _targetText = commonStrings.GetString("UI/Informator/TargetButton");
            _untargetText = commonStrings.GetString("UI/Informator/UntargetButton");
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _userChosenTargetHelper.ToggleChosenTarget(_target);
        }

        private void UpdateButtonText()
        {
            _buttonText.text = ReferenceEquals(_target, _userChosenTargetHelper.UserChosenTarget) ? _untargetText : _targetText;
        }
    }
}

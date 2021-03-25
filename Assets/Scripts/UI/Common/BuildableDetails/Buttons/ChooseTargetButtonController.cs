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
        private IUserChosenTargetHelper _userChosenTargetHelper;
        private IFilter<ITarget> _buttonVisibilityFilter;

        public Image activeFeedback;

        private ITarget _target;
        public ITarget Target
        {
            private get { return _target; }
            set
            {
                _target = value;

                gameObject.SetActive(ShowButton);
                UpdateActiveFeedback();
            }
        }

        private bool ShowButton => _buttonVisibilityFilter.IsMatch(Target);

        public void Initialise(
            ISingleSoundPlayer soundPlayer, 
            IUserChosenTargetHelper userChosenTargetHelper, 
            IFilter<ITarget> buttonVisibilityFilter)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(userChosenTargetHelper, buttonVisibilityFilter);
            Assert.IsNotNull(activeFeedback);

            _userChosenTargetHelper = userChosenTargetHelper;
            _buttonVisibilityFilter = buttonVisibilityFilter;

            _userChosenTargetHelper.UserChosenTargetChanged += (sender, e) => UpdateActiveFeedback();
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _userChosenTargetHelper.ToggleChosenTarget(_target);
        }

        private void UpdateActiveFeedback()
        {
            activeFeedback.gameObject.SetActive(IsUserChosenTarget());
        }

        private bool IsUserChosenTarget()
        {
            return ReferenceEquals(_target, _userChosenTargetHelper.UserChosenTarget);
        }
    }
}

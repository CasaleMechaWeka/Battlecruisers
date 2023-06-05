using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    public class PvPChooseTargetButtonController : PvPCanvasGroupButton, IPvPButton
    {
        private IPvPUserChosenTargetHelper _userChosenTargetHelper;
        private IPvPFilter<IPvPTarget> _buttonVisibilityFilter;

        public Image activeFeedback;

        private IPvPTarget _target;
        public IPvPTarget Target
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
            IPvPSingleSoundPlayer soundPlayer,
            IPvPUserChosenTargetHelper userChosenTargetHelper,
            IPvPFilter<IPvPTarget> buttonVisibilityFilter)
        {
            base.Initialise(soundPlayer);

            PvPHelper.AssertIsNotNull(userChosenTargetHelper, buttonVisibilityFilter);
            Assert.IsNotNull(activeFeedback);

            _userChosenTargetHelper = userChosenTargetHelper;
            _buttonVisibilityFilter = buttonVisibilityFilter;

            _userChosenTargetHelper.UserChosenTargetChanged += (sender, e) => UpdateActiveFeedback();
        }


        public void Initialise(
            IPvPSingleSoundPlayer soundPlayer,
            // IPvPUserChosenTargetHelper userChosenTargetHelper,
            IPvPFilter<IPvPTarget> buttonVisibilityFilter)
        {
            base.Initialise(soundPlayer);

            // PvPHelper.AssertIsNotNull(userChosenTargetHelper, buttonVisibilityFilter);
            Assert.IsNotNull(activeFeedback);

            // _userChosenTargetHelper = userChosenTargetHelper;
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

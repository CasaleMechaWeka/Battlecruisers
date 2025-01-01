using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    public class PvPChooseTargetButtonController : PvPCanvasGroupButton, IPvPButton
    {
        private IPvPUserChosenTargetHelper _userChosenTargetHelper;
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

        private bool ShowButton => Target != null && (SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT ? _target.Faction == Faction.Reds : _target.Faction == Faction.Blues) && _buttonVisibilityFilter.IsMatch(Target);

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IPvPUserChosenTargetHelper userChosenTargetHelper,
            IFilter<ITarget> buttonVisibilityFilter)
        {
            base.Initialise(soundPlayer);

            PvPHelper.AssertIsNotNull(userChosenTargetHelper, buttonVisibilityFilter);
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

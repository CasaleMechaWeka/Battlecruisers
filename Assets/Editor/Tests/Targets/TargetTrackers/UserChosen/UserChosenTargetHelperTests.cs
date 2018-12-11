using BattleCruisers.Buildables;
using BattleCruisers.Data.Static;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.Sound;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetTrackers.UserChosen
{
    public class UserChosenTargetHelperTests
    {
        private IUserChosenTargetHelper _targetHelper;
        private IUserChosenTargetManager _targetManager;
        private IPrioritisedSoundPlayer _soundPlayer;
        private RankedTarget _target1, _target2;

        [SetUp]
        public void TestSetup()
        {
            _targetManager = Substitute.For<IUserChosenTargetManager>();
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();
            _targetHelper = new UserChosenTargetHelper(_targetManager, _soundPlayer);

            _target1 = new RankedTarget(Substitute.For<ITarget>(), rank: 17);
            _target2 = new RankedTarget(Substitute.For<ITarget>(), rank: 71);
        }

        [Test]
        public void ToggleChosenTarget_CurrentTargetIsNull_ChoosesGivenTarget()
        {
            _targetManager.HighestPriorityTarget.Returns((RankedTarget)null);
            _targetHelper.ToggleChosenTarget(_target1.Target);
            _targetManager.Received().Target = _target1.Target;
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Targetting.NewTarget);
        }

        [Test]
        public void ToggleChosenTarget_CurrentTargetIsDifferent_ChoosesGivenTarget()
        {
            _targetManager.HighestPriorityTarget.Returns(_target1);
            _targetHelper.ToggleChosenTarget(_target2.Target);
            _targetManager.Received().Target = _target2.Target;
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Targetting.NewTarget);
        }

        [Test]
        public void ToggleChosenTarget_CurrentTargetIsSame_ClearsChosenTarget()
        {
            _targetManager.HighestPriorityTarget.Returns(_target1);
            _targetHelper.ToggleChosenTarget(_target1.Target);
            _targetManager.Received().Target = null;
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Targetting.TargetCleared);
        }
    }
}
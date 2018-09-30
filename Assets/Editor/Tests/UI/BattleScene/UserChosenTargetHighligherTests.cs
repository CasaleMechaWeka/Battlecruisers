using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene
{
    public class UserChosenTargetHighligherTests
    {
        private UserChosenTargetHighligher _highlighter;
        private IRankedTargetTracker _userChosenTargetTracker;
        private IHighlightHelper _highlightHelper;
        private IHighlight _highlight1, _highlight2;
        private ITarget _target1, _target2;

        [SetUp]
        public void TestSetup()
        {
            _userChosenTargetTracker = Substitute.For<IRankedTargetTracker>();
            _highlightHelper = Substitute.For<IHighlightHelper>();

            _highlighter = new UserChosenTargetHighligher(_userChosenTargetTracker, _highlightHelper);

            _target1 = Substitute.For<ITarget>();
            _highlight1 = Substitute.For<IHighlight>();
            _highlightHelper.CreateHighlight(_target1, usePulsingAnimation: false).Returns(_highlight1);

            _target2 = Substitute.For<ITarget>();
            _highlight2 = Substitute.For<IHighlight>();
            _highlightHelper.CreateHighlight(_target2, usePulsingAnimation: false).Returns(_highlight2);
        }

        [Test]
        public void UserChoosesTarget_HighlightsTarget()
        {
            ChooseTarget(_target1);
            _highlightHelper.Received().CreateHighlight(_target1, usePulsingAnimation: false);
        }

        [Test]
        public void UserClearsTarget_Unhighlights()
        {
            // User chooses target
            ChooseTarget(_target1);

            // User clears target
            ChooseTarget(null);
            _highlight1.Received().Destroy();
        }

        [Test]
        public void UserChangesTarget_UnhighlightsCurrentTarget_HighlightsNewTarget()
        {
            // User chooses a target
            ChooseTarget(_target1);

            // User chooses a different target
            ChooseTarget(_target2);
            _highlight1.Received().Destroy();
            _highlightHelper.Received().CreateHighlight(_target2, usePulsingAnimation: false);
        }

        [Test]
        public void Dispose_Unsubsribes()
        {
            _highlighter.DisposeManagedState();

            ChooseTarget(_target1);

            _highlightHelper.DidNotReceiveWithAnyArgs().CreateHighlight(null);
        }

        private void ChooseTarget(ITarget target)
        {
            RankedTarget userChosenTarget = target != null ? new RankedTarget(target, rank: 17) : null;
            _userChosenTargetTracker.HighestPriorityTarget.Returns(userChosenTarget);
            _userChosenTargetTracker.HighestPriorityTargetChanged += Raise.Event();
        }
    }
}
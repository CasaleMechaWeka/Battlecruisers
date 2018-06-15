using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Tutorial.Steps.WaitSteps
{
    public class BuildableCompletedWaitStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _tutorialStep;

        private IItemProvider<IBuildable> _buildableProvider;
        private IBuildable _buildable;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _buildable = Substitute.For<IBuildable>();
            _buildableProvider = Substitute.For<IItemProvider<IBuildable>>();
            _buildableProvider.FindItem().Returns(_buildable);

            _tutorialStep = new BuildableCompletedWaitStep(_args, _buildableProvider);
        }

        #region Start
        [Test]
        public void Start()
        {
            _tutorialStep.Start(_completionCallback);
            _buildableProvider.Received().FindItem();
        }

        [Test]
        public void Start_NullBuildableProvided_Throws()
        {
            _buildableProvider.FindItem().Returns((IBuildable)null);
            Assert.Throws<UnityAsserts.AssertionException>(() => _tutorialStep.Start(_completionCallback));
        }
        #endregion Start

        [Test]
        public void BuildableConstructionCompletes_TriggersCompletedCallback()
        {
            Start();

            _buildable.CompletedBuildable += Raise.Event();
            Assert.AreEqual(1, _callbackCounter);
        }
    }
}

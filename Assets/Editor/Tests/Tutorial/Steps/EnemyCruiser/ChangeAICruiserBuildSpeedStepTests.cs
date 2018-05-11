using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.EnemyCruiser;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.EnemyCruiser
{
    public class ChangeAICruiserBuildSpeedStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _tutorialStep;
        private IBuildSpeedController _buildSpeedController;
        private BuildSpeed _buildSpeed;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _buildSpeedController = Substitute.For<IBuildSpeedController>();
            _buildSpeed = BuildSpeed.VeryFast;
            _tutorialStep = new ChangeAICruiserBuildSpeedStep(_args, _buildSpeedController, _buildSpeed);
        }

        [Test]
        public void Start_SetsSpeed_AndCompletes()
        {
            _tutorialStep.Start(_completionCallback);
            _buildSpeedController.Received().BuildSpeed = _buildSpeed;
            Assert.AreEqual(1, _callbackCounter);
        }
    }
}

using BattleCruisers.Tutorial.Steps;
using BattleCruisers.UI.BattleScene.Navigation;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps
{
    public class CameraFocuserStepTests : TutorialStepTestsBase
    {
        private ICameraFocuser _cameraFocuser;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _cameraFocuser = Substitute.For<ICameraFocuser>();
        }

        [Test]
        public void Start_PlayerCruiserTarget()
        {
            ITutorialStep step = new CameraFocuserStep(_args, _cameraFocuser, CameraFocuserTarget.PlayerCruiser);
            step.Start(_completionCallback);

            _cameraFocuser.Received().FocusOnPlayerCruiser();
            Assert.AreEqual(1, _callbackCounter);
        }

        [Test]
        public void Start_AICruiserTarget()
        {
            ITutorialStep step = new CameraFocuserStep(_args, _cameraFocuser, CameraFocuserTarget.AICruiser);
            step.Start(_completionCallback);

            _cameraFocuser.Received().FocusOnAICruiser();
            Assert.AreEqual(1, _callbackCounter);
        }

        [Test]
        public void Start_AICruiserNavalFactoryTarget()
        {
            ITutorialStep step = new CameraFocuserStep(_args, _cameraFocuser, CameraFocuserTarget.AICruiserNavalFactory);
            step.Start(_completionCallback);

            _cameraFocuser.Received().FocusOnAINavalFactory();
            Assert.AreEqual(1, _callbackCounter);
        }

        [Test]
        public void Start_MidLeftTarget()
        {
            ITutorialStep step = new CameraFocuserStep(_args, _cameraFocuser, CameraFocuserTarget.MidLeft);
            step.Start(_completionCallback);

            _cameraFocuser.Received().FocusMidLeft();
            Assert.AreEqual(1, _callbackCounter);
        }
    }
}

using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Navigation
{
    public class CameraFocuserTests
    {
        private ICameraFocuser _cameraFocuser;
        private ICameraTargets _targets;
        private IStaticCameraTargetProvider _trumpCameraTargetProvider, _defaultCameraTargetProvider;
        private ICameraTransitionSpeedManager _cameraTransitionSpeedManager;
        private ICameraTarget _target;

        [SetUp]
        public void TestSetup()
        {
            _targets = Substitute.For<ICameraTargets>();
            _trumpCameraTargetProvider = Substitute.For<IStaticCameraTargetProvider>();
            _defaultCameraTargetProvider = Substitute.For<IStaticCameraTargetProvider>();
            _cameraTransitionSpeedManager = Substitute.For<ICameraTransitionSpeedManager>();

            _cameraFocuser = new CameraFocuser(_targets, _trumpCameraTargetProvider, _defaultCameraTargetProvider, _cameraTransitionSpeedManager);

            _target = Substitute.For<ICameraTarget>();
        }

        [Test]
        public void FocusOnPlayerCruiser()
        {
            _targets.PlayerCruiserTarget.Returns(_target);
            
            _cameraFocuser.FocusOnPlayerCruiser();

            _cameraTransitionSpeedManager.Received().SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.Received().SetTarget(_target);
        }

        [Test]
        public void FocusOnPlayerCruiserZoomedOut()
        {
            _targets.PlayerCruiserDeathTarget.Returns(_target);

            _cameraFocuser.FocusOnPlayerCruiserDeath();
            
            _cameraTransitionSpeedManager.Received().SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.Received().SetTarget(_target);
        }

        [Test]
        public void FocusOnPlayerNavalFactory()
        {
            _targets.PlayerNavalFactoryTarget.Returns(_target);
            
            _cameraFocuser.FocusOnPlayerNavalFactory();
            
            _cameraTransitionSpeedManager.Received().SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.Received().SetTarget(_target);
        }

        [Test]
        public void FocusOnAICruiser()
        {
            _targets.AICruiserTarget.Returns(_target);
            
            _cameraFocuser.FocusOnAICruiser();
            
            _cameraTransitionSpeedManager.Received().SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.Received().SetTarget(_target);
        }

        [Test]
        public void FocusOnAICruiserZoomedOut()
        {
            _targets.AICruiserDeathTarget.Returns(_target);
            
            _cameraFocuser.FocusOnAICruiserDeath();
            
            _cameraTransitionSpeedManager.Received().SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.Received().SetTarget(_target);
        }

        [Test]
        public void FocusOnAINavalFactory()
        {
            _targets.AINavalFactoryTarget.Returns(_target);
            
            _cameraFocuser.FocusOnAINavalFactory();
            
            _cameraTransitionSpeedManager.Received().SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.Received().SetTarget(_target);
        }

        [Test]
        public void FocusOnMidLeft()
        {
            _targets.MidLeftTarget.Returns(_target);
            
            _cameraFocuser.FocusMidLeft();
            
            _cameraTransitionSpeedManager.Received().SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.Received().SetTarget(_target);
        }

        [Test]
        public void FocusOnOverview()
        {
            _targets.OverviewTarget.Returns(_target);
            
            _cameraFocuser.FocusOnOverview();
            
            _cameraTransitionSpeedManager.Received().SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.Received().SetTarget(_target);
        }

        [Test]
        public void FocusOnPlayerCruiserNuke()
        {
            _targets.PlayerCruiserNukedTarget.Returns(_target);
            
            _cameraFocuser.FocusOnPlayerCruiserNuke();
            
            _cameraTransitionSpeedManager.Received().SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.Received().SetTarget(_target);
        }

        [Test]
        public void FocusOnAICruiserNuke()
        {
            _targets.AICruiserNukedTarget.Returns(_target);
            
            _cameraFocuser.FocusOnAICruiserNuke();
            
            _cameraTransitionSpeedManager.Received().SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.Received().SetTarget(_target);
        }
    }
}
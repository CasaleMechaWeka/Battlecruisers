using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Providers
{
    public class CompositeCameraTargetProviderTests
    {
        private ICameraTargetProvider _compositeTargetProvider;
        private IUserInputCameraTargetProvider _primaryTargetProvider, _secondaryTargetProvider;
        private INavigationWheel _navigationWheel;
        private ICameraNavigationWheelCalculator _navigationWheelCalculator;
        private ICameraTarget _primaryTarget, _secondaryTarget;
        private int _targetChangedCount;

        [SetUp]
        public void TestSetup()
        {
            _primaryTargetProvider = Substitute.For<IUserInputCameraTargetProvider>();
            _secondaryTargetProvider = Substitute.For<IUserInputCameraTargetProvider>();
            _navigationWheel = Substitute.For<INavigationWheel>();
            _navigationWheelCalculator = Substitute.For<ICameraNavigationWheelCalculator>();

            _compositeTargetProvider
                = new CompositeCameraTargetProvider(
                    _primaryTargetProvider,
                    _secondaryTargetProvider,
                    // FELIX  Fix :)
                    null,
                    _navigationWheel,
                    _navigationWheelCalculator);

            _targetChangedCount = 0;
            _compositeTargetProvider.TargetChanged += (sender, e) => _targetChangedCount++;

            _primaryTarget = Substitute.For<ICameraTarget>();
            _primaryTargetProvider.Target.Returns(_primaryTarget);

            _secondaryTarget = Substitute.For<ICameraTarget>();
            _secondaryTargetProvider.Target.Returns(_secondaryTarget);
        }

        [Test]
        public void InitialState()
        {
            // Navigatoin wheel starts as active target provider
            Assert.AreSame(_primaryTarget, _compositeTargetProvider.Target);
        }

        [Test]
        public void PrimaryActive()
        {
            // Primary target changed => forwarded
            _primaryTargetProvider.TargetChanged += Raise.Event();
            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreSame(_primaryTarget, _compositeTargetProvider.Target);

            // Secondary target changed => ignored
            _secondaryTargetProvider.TargetChanged += Raise.Event();
            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreNotSame(_secondaryTarget, _compositeTargetProvider.Target);
        }

        [Test]
        public void UserInputActive()
        {
            _secondaryTargetProvider.UserInputStarted += Raise.Event();

            // Secondary target changed => forwarded
            _secondaryTargetProvider.TargetChanged += Raise.Event();
            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreSame(_secondaryTarget, _compositeTargetProvider.Target);

            // Primary target changed => forwarded
            _primaryTargetProvider.TargetChanged += Raise.Event();
            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreNotSame(_primaryTarget, _compositeTargetProvider.Target);
        }

        [Test]
        public void UserInputStarted_BecomesActive()
        {
            _secondaryTargetProvider.UserInputStarted += Raise.Event();
            Assert.AreSame(_secondaryTarget, _compositeTargetProvider.Target);
        }

        [Test]
        public void UserInputEnded_BecomesInactive()
        {
            Vector2 navigationWheelPosition = new Vector2(12, 21);
            _navigationWheelCalculator.FindNavigationWheelPosition(_secondaryTarget).Returns(navigationWheelPosition);
            
            // Secondary provider becomes active
            _secondaryTargetProvider.UserInputStarted += Raise.Event();

            // Secondary provider becomes inactive
            _secondaryTargetProvider.UserInputEnded += Raise.Event();

            Assert.AreNotSame(_secondaryTarget, _compositeTargetProvider.Target);
            _navigationWheel.Received().SetCenterPosition(navigationWheelPosition, snapToCorners: false);
        }
    }
}
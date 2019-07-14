using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Helpers;
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
        private IUserInputCameraTargetProvider _navigationWheelTargetProvider, _scrollWheelTargetProvider;
        private INavigationWheel _navigationWheel;
        private ICameraNavigationWheelCalculator _navigationWheelCalculator;
        private ICameraTarget _navigationWheelTarget, _scrollWheelTarget;
        private int _targetChangedCount;

        [SetUp]
        public void TestSetup()
        {
            _navigationWheelTargetProvider = Substitute.For<IUserInputCameraTargetProvider>();
            _scrollWheelTargetProvider = Substitute.For<IUserInputCameraTargetProvider>();
            _navigationWheel = Substitute.For<INavigationWheel>();
            _navigationWheelCalculator = Substitute.For<ICameraNavigationWheelCalculator>();

            _compositeTargetProvider
                = new CompositeCameraTargetProvider(
                    _navigationWheelTargetProvider,
                    _scrollWheelTargetProvider,
                    _navigationWheel,
                    _navigationWheelCalculator);

            _targetChangedCount = 0;
            _compositeTargetProvider.TargetChanged += (sender, e) => _targetChangedCount++;

            _navigationWheelTarget = Substitute.For<ICameraTarget>();
            _navigationWheelTargetProvider.Target.Returns(_navigationWheelTarget);

            _scrollWheelTarget = Substitute.For<ICameraTarget>();
            _scrollWheelTargetProvider.Target.Returns(_scrollWheelTarget);
        }

        [Test]
        public void InitialState()
        {
            // Navigatoin wheel starts as active target provider
            Assert.AreSame(_navigationWheelTarget, _compositeTargetProvider.Target);
        }

        [Test]
        public void NavigationWheelActive()
        {
            // Navigation wheel target changed => forwarded
            _navigationWheelTargetProvider.TargetChanged += Raise.Event();
            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreSame(_navigationWheelTarget, _compositeTargetProvider.Target);

            // Scroll wheel target changed => ignored
            _scrollWheelTargetProvider.TargetChanged += Raise.Event();
            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreNotSame(_scrollWheelTarget, _compositeTargetProvider.Target);
        }

        [Test]
        public void UserInputActive()
        {
            _scrollWheelTargetProvider.UserInputStarted += Raise.Event();

            // Scroll wheel target changed => forwarded
            _scrollWheelTargetProvider.TargetChanged += Raise.Event();
            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreSame(_scrollWheelTarget, _compositeTargetProvider.Target);

            // Navigation wheel target changed => forwarded
            _navigationWheelTargetProvider.TargetChanged += Raise.Event();
            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreNotSame(_navigationWheelTarget, _compositeTargetProvider.Target);
        }

        [Test]
        public void UserInputStarted_BecomesActive()
        {
            _scrollWheelTargetProvider.UserInputStarted += Raise.Event();
            Assert.AreSame(_scrollWheelTarget, _compositeTargetProvider.Target);
        }

        [Test]
        public void UserInputEnded_BecomesInactive()
        {
            Vector2 navigationWheelPosition = new Vector2(12, 21);
            _navigationWheelCalculator.FindNavigationWheelPosition(_scrollWheelTarget).Returns(navigationWheelPosition);
            
            // Scroll wheel provider becomes active
            _scrollWheelTargetProvider.UserInputStarted += Raise.Event();

            // Scroll wheel provider becomes inactive
            _scrollWheelTargetProvider.UserInputEnded += Raise.Event();

            Assert.AreNotSame(_scrollWheelTarget, _compositeTargetProvider.Target);
            _navigationWheel.Received().CenterPosition = navigationWheelPosition;
        }
    }
}
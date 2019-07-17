using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.UI.Cameras.Targets.Providers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Providers
{
    public class NavigationWheelCameraTargetProviderTests
    {
        private ICameraTargetProvider _cameraTargetProvider;
        private INavigationWheel _navigationWheel;
        private ICameraTargetFinder _cameraTargetFinder, _cornersCameraTargetFinder;
        private ICameraTarget _target1, _target2;
        private int _targetChangedCount;

        [SetUp]
        public void TestSetup()
        {
            _target1 = Substitute.For<ICameraTarget>();
            _target2 = Substitute.For<ICameraTarget>();

            _navigationWheel = Substitute.For<INavigationWheel>();

            _cameraTargetFinder = Substitute.For<ICameraTargetFinder>();
            _cameraTargetFinder.FindCameraTarget().Returns(_target2);
            _cornersCameraTargetFinder = Substitute.For<ICameraTargetFinder>();
            _cornersCameraTargetFinder.FindCameraTarget().Returns(_target1, _target2);

            _cameraTargetProvider = new NavigationWheelCameraTargetProvider(_navigationWheel, _cameraTargetFinder, _cornersCameraTargetFinder);

            _targetChangedCount = 0;
            _cameraTargetProvider.TargetChanged += (sender, e) => _targetChangedCount++;
        }

        [Test]
        public void Constructor_FindsTarget_CornersFinder()
        {
            _cornersCameraTargetFinder.Received().FindCameraTarget();
            Assert.AreSame(_target1, _cameraTargetProvider.Target);
        }

        [Test]
        public void NavigationWheelCenterPositionChanged_FindsNewTarget_CornersFinder()
        {
            _cornersCameraTargetFinder.ClearReceivedCalls();
            _navigationWheel.CenterPositionChanged += Raise.EventWith(new PositionChangedEventArgs(PositionChangeSource.NavigationWheel));

            _cornersCameraTargetFinder.Received().FindCameraTarget();
            _cameraTargetFinder.DidNotReceive().FindCameraTarget();
            Assert.AreSame(_target2, _cameraTargetProvider.Target);
            Assert.AreEqual(1, _targetChangedCount);
        }

        [Test]
        public void NavigationWheelCenterPositionChanged_FindsNewTarget_NormalFinder()
        {
            _cornersCameraTargetFinder.ClearReceivedCalls();
            _navigationWheel.CenterPositionChanged += Raise.EventWith(new PositionChangedEventArgs(PositionChangeSource.Other));

            _cameraTargetFinder.Received().FindCameraTarget();
            _cornersCameraTargetFinder.DidNotReceive().FindCameraTarget();
            Assert.AreSame(_target2, _cameraTargetProvider.Target);
            Assert.AreEqual(1, _targetChangedCount);
        }
    }
}
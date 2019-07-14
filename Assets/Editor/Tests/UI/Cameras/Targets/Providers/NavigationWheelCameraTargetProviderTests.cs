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
        private ICameraTargetFinder _cameraTargetFinder;
        private ICameraTarget _target1, _target2;
        private int _targetChangedCount;

        [SetUp]
        public void TestSetup()
        {
            _target1 = Substitute.For<ICameraTarget>();
            _target2 = Substitute.For<ICameraTarget>();

            _navigationWheel = Substitute.For<INavigationWheel>();

            _cameraTargetFinder = Substitute.For<ICameraTargetFinder>();
            _cameraTargetFinder.FindCameraTarget().Returns(_target1, _target2);

            _cameraTargetProvider = new NavigationWheelCameraTargetProvider(_navigationWheel, _cameraTargetFinder);

            _targetChangedCount = 0;
            _cameraTargetProvider.TargetChanged += (sender, e) => _targetChangedCount++;
        }

        [Test]
        public void Constructor_FindsTarget()
        {
            _cameraTargetFinder.Received().FindCameraTarget();
            Assert.AreSame(_target1, _cameraTargetProvider.Target);
        }

        [Test]
        public void NavigationWheelCenterPositionChanged_FindsNewTarget()
        {
            _cameraTargetFinder.ClearReceivedCalls();
            _navigationWheel.CenterPositionChanged += Raise.EventWith(new PositionChangedEventArgs(PositionChangeSource.Other));

            _cameraTargetFinder.Received().FindCameraTarget();
            Assert.AreSame(_target2, _cameraTargetProvider.Target);
            Assert.AreEqual(1, _targetChangedCount);
        }
    }
}
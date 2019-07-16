using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Finders
{
    public class NavigationWheelCameraTargetFinderTests
    {
        private ICameraTargetFinder _cameraTargetFinder;
        private ICameraNavigationWheelCalculator _calculator;
        private ICamera _camera;

        [SetUp]
        public void TestSetup()
        {
            _calculator = Substitute.For<ICameraNavigationWheelCalculator>();
            _camera = Substitute.For<ICamera>();
            _cameraTargetFinder = new NavigationWheelCameraTargetFinder(_calculator, _camera);
        }

        [Test]
        public void FindCameraTarget()
        {
            _camera.Transform.Position.Returns(new Vector3(-12, -11, -10));

            Vector2 cameraTargetPosition = new Vector2(12, 13);
            _calculator.FindCameraPosition().Returns(cameraTargetPosition);

            float targetOthographicSize = 67;
            _calculator.FindOrthographicSize().Returns(targetOthographicSize);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(cameraTargetPosition.x, cameraTargetPosition.y, _camera.Transform.Position.z),
                    targetOthographicSize);

            Assert.AreEqual(expectedTarget, _cameraTargetFinder.FindCameraTarget());
        }
    }
}
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Helpers
{
    public class EdgeDetectorTests
    {
        private IEdgeDetector _detector;
        private IInput _input;
        private int _edgeRegionWithInPixels;

        [SetUp]
        public void TestSetup()
        {
            _input = Substitute.For<IInput>();
            _edgeRegionWithInPixels = 5;

            _detector = new EdgeDetector(_input, _edgeRegionWithInPixels);
        }

        [Test]
        public void IsCursorAtLeftEdge_True()
        {
            _input.MousePosition.Returns(new Vector3(_edgeRegionWithInPixels - 1, 0, 0));
            Assert.IsTrue(_detector.IsCursorAtLeftEdge());
        }

        [Test]
        public void IsCursorAtLeftEdge_False()
        {
            _input.MousePosition.Returns(new Vector3(_edgeRegionWithInPixels, 0, 0));
            Assert.IsFalse(_detector.IsCursorAtLeftEdge());
        }

        [Test]
        public void IsCursorAtRightEdge_True()
        {
            Vector3 mousePosition = new Vector3(Screen.width - _edgeRegionWithInPixels + 1, 0, 0);
            _input.MousePosition.Returns(mousePosition);
            Assert.IsTrue(_detector.IsCursorAtRightEdge());
        }

        [Test]
        public void IsCursorAtRightEdge_False()
        {
            Vector3 mousePosition = new Vector3(Screen.width - _edgeRegionWithInPixels, 0, 0);
            _input.MousePosition.Returns(mousePosition);
            Assert.IsFalse(_detector.IsCursorAtRightEdge());
        }
    }
}
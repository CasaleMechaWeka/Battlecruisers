using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Arrows;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Tutorial.Highlighting.Arrows
{
    public class ArrowCalculatorTests
    {
        private IArrowCalculator _calculator;
        private ICamera _camera;
        private HighlightArgs _args;

        [SetUp]
        public void TestSetup()
        {
            _camera = Substitute.For<ICamera>();
            _calculator = new ArrowCalculator(_camera);

            _camera.PixelWidth.Returns(1920);
            _camera.PixelHeight.Returns(1080);

            _args
                = new HighlightArgs(
                    centerPosition: new Vector2(2, 3),
                    bottomLeftPosition: default,  // Not used
                    size: new Vector2(4, 4));
        }

        [Test]
        public void ShouldShowArrow_True()
        {
            Vector2 size = new Vector2(ArrowCalculator.HIGHLIGHTABLE_CUTOFF_SIZE_IN_PIXELS - 1, 1);
            Assert.IsTrue(_calculator.ShouldShowArrow(size));
        }

        [Test]
        public void ShouldShowArrow_False()
        {
            Vector2 size = new Vector2(ArrowCalculator.HIGHLIGHTABLE_CUTOFF_SIZE_IN_PIXELS, 1);
            Assert.IsFalse(_calculator.ShouldShowArrow(size));
        }

        #region FindArrowDirection
        [Test]
        public void FindArrowDirection_North()
        {
            Vector2 centerPosition = new Vector2(_camera.PixelWidth / 2, 17);
            Assert.AreEqual(ArrowDirection.North, _calculator.FindArrowDirection(centerPosition));
        }

        [Test]
        public void FindArrowDirection_SouthWest()
        {
            Vector2 centerPosition = new Vector2(_camera.PixelWidth / 2 - 1, _camera.PixelHeight / 2 - 1);
            Assert.AreEqual(ArrowDirection.SouthWest, _calculator.FindArrowDirection(centerPosition));
        }

        [Test]
        public void FindArrowDirection_NorthWest()
        {
            Vector2 centerPosition = new Vector2(_camera.PixelWidth / 2 - 1, _camera.PixelHeight / 2 +  1);
            Assert.AreEqual(ArrowDirection.NorthWest, _calculator.FindArrowDirection(centerPosition));
        }

        [Test]
        public void FindArrowDirection_SouthEast()
        {
            Vector2 centerPosition = new Vector2(_camera.PixelWidth / 2 + 1, _camera.PixelHeight / 2 - 1);
            Assert.AreEqual(ArrowDirection.SouthEast, _calculator.FindArrowDirection(centerPosition));
        }

        [Test]
        public void FindArrowDirection_NorthEast()
        {
            Vector2 centerPosition = new Vector2(_camera.PixelWidth / 2 + 1, _camera.PixelHeight / 2 + 1);
            Assert.AreEqual(ArrowDirection.NorthEast, _calculator.FindArrowDirection(centerPosition));
        }
        #endregion FindArrowDirection

        #region FindArrowHeadPosition
        [Test]
        public void FindArrowHeadPosition_North()
        {
            Vector2 expectedPosition
                = new Vector2(
                    _args.CenterPosition.x,
                    _args.CenterPosition.y - _args.Size.y / 2);
            Assert.AreEqual(expectedPosition, _calculator.FindArrowHeadPosition(_args, ArrowDirection.North));
        }

        [Test]
        public void FindArrowHeadPosition_NorthEast()
        {
            Vector2 expectedPosition
                = new Vector2(
                    _args.CenterPosition.x - _args.Size.x / 2,
                    _args.CenterPosition.y - _args.Size.y / 2);
            Assert.AreEqual(expectedPosition, _calculator.FindArrowHeadPosition(_args, ArrowDirection.NorthEast));
        }
        
        [Test]
        public void FindArrowHeadPosition_NorthWest()
        {
            Vector2 expectedPosition
                = new Vector2(
                    _args.CenterPosition.x + _args.Size.x / 2,
                    _args.CenterPosition.y - _args.Size.y / 2);
            Assert.AreEqual(expectedPosition, _calculator.FindArrowHeadPosition(_args, ArrowDirection.NorthWest));
        }
        
        [Test]
        public void FindArrowHeadPosition_SouthEast()
        {
            Vector2 expectedPosition
                = new Vector2(
                    _args.CenterPosition.x - _args.Size.x / 2,
                    _args.CenterPosition.y + _args.Size.y / 2);
            Assert.AreEqual(expectedPosition, _calculator.FindArrowHeadPosition(_args, ArrowDirection.SouthEast));
        }

        [Test]
        public void FindArrowHeadPosition_SouthWest()
        {
            Vector2 expectedPosition
                = new Vector2(
                    _args.CenterPosition.x + _args.Size.x / 2,
                    _args.CenterPosition.y + _args.Size.y / 2);
            Assert.AreEqual(expectedPosition, _calculator.FindArrowHeadPosition(_args, ArrowDirection.SouthWest));
        }
        #endregion FindArrowHeadPosition

        [Test]
        public void FindArrowDirectionVector()
        {
            Vector2 arrowHead = new Vector2(1, 3);
            Vector2 centerPosition = new Vector2(5, 4);
            Vector2 expectedDirection = centerPosition - arrowHead;

            Assert.AreEqual(expectedDirection, _calculator.FindArrowDirectionVector(arrowHead, centerPosition));
        }
    }
}
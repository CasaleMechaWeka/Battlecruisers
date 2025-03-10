using BattleCruisers.Utils.DataStrctures;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Utils.DataStructures
{
    public class PyramidTests
    {
        private IPyramid _pyramid;
        private Vector2 _bottomLeftVertex, _bottomRightVertex, _topCenterVertex;

        [SetUp]
        public void TestSetup()
        {
            _bottomLeftVertex = new Vector2(-2, 0);
            _bottomRightVertex = new Vector2(2, 0);
            _topCenterVertex = new Vector2(0, 4);

            _pyramid = new Pyramid(_bottomLeftVertex, _bottomRightVertex, _topCenterVertex);
        }

        #region Constructor
        [Test]
        public void Constructor_TopCenterVertex_IsToLeftOfBottomLeftVertex_Throws()
        {
            _topCenterVertex = new Vector2(-3, 4);
            Assert.Throws<UnityAsserts.AssertionException>(() => new Pyramid(_bottomLeftVertex, _bottomRightVertex, _topCenterVertex));
        }

        [Test]
        public void Constructor_TopCenterVertex_IsToRightOfBottomRightVertex_Throws()
        {
            _topCenterVertex = new Vector2(3, 4);
            Assert.Throws<UnityAsserts.AssertionException>(() => new Pyramid(_bottomLeftVertex, _bottomRightVertex, _topCenterVertex));
        }

        [Test]
        public void Constructor_BottomVerticesAreNotAtSameHeight_Throws()
        {
            _bottomLeftVertex = new Vector2(-2, 0.01f);
            Assert.Throws<UnityAsserts.AssertionException>(() => new Pyramid(_bottomLeftVertex, _bottomRightVertex, _topCenterVertex));
        }

        [Test]
        public void Constructor_TopCenterVertex_IsNotAboveBottomVertices_Throws()
        {
            _topCenterVertex = new Vector2(0, -0.01f);
            Assert.Throws<UnityAsserts.AssertionException>(() => new Pyramid(_bottomLeftVertex, _bottomRightVertex, _topCenterVertex));
        }

        [Test]
        public void Constructor_TopCenterVertex_IsNotInTheMiddleOfBototmVertices_Throws()
        {
            _topCenterVertex = new Vector2(0.01f, 4);
            Assert.Throws<UnityAsserts.AssertionException>(() => new Pyramid(_bottomLeftVertex, _bottomRightVertex, _topCenterVertex));
        }
        #endregion Constructor

        #region FindMaxY
        [Test]
        public void FindMaxY_TooSmallX_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _pyramid.FindMaxY(_bottomLeftVertex.x - 0.01f));
        }

        [Test]
        public void FindMaxY_TooBigX_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _pyramid.FindMaxY(_bottomRightVertex.x + 0.01f));
        }

        [Test]
        public void FindMaxY_ValidX_FindsMaxY()
        {
            float xPosition = -1;
            float expectedMaxY = 2;
            Assert.AreEqual(expectedMaxY, _pyramid.FindMaxY(xPosition));
        }
        #endregion FindMaxY

        #region FindGlobalXRange
        [Test]
        public void FindGlobalXRange_TooSmallYPosition_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _pyramid.FindGlobalXRange(localYPosition: -0.01f));
        }

        [Test]
        public void FindGlobalXRange_TooBigYPosition_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _pyramid.FindGlobalXRange(_topCenterVertex.y + 0.01f));
        }

        [Test]
        public void FindGlobalXRange_ValidYPosition_FindsGlobalXRange()
        {
            IRange<float> expectedGlobalXRange = new Range<float>(min: -1, max: 1);
            Assert.AreEqual(expectedGlobalXRange, _pyramid.FindGlobalXRange(localYPosition: 2));
        }
        #endregion FindGlobalXRange
    }
}
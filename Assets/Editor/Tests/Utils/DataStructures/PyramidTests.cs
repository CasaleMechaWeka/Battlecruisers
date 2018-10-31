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
            UnityAsserts.Assert.raiseExceptions = true;

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
    }
}
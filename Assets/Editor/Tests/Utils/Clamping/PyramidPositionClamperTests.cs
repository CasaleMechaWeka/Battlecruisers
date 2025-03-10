using BattleCruisers.Utils.Clamping;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Utils.Clamping
{
    public class PyramidPositionClamperTests
    {
        private IPositionClamper _clamper;
        private IPyramid _pyramid;

        [SetUp]
        public void TestSetup()
        {
            _pyramid = Substitute.For<IPyramid>();
            _clamper = new PyramidPositionClamper(_pyramid);
        }

        [Test]
        public void Clamp()
        {
            _pyramid.BottomLeftVertex.Returns(new Vector2(-2, 0));
            _pyramid.BottomRightVertex.Returns(new Vector2(2, 0));
            float maxY = 4;
            _pyramid.FindMaxY(default).ReturnsForAnyArgs(maxY);

            Vector2 positionToClamp = new Vector2(_pyramid.BottomLeftVertex.x - 0.01f, maxY + 0.01f);
            Vector2 expectedClampedResult = new Vector2(_pyramid.BottomLeftVertex.x, maxY);
            Assert.AreEqual(expectedClampedResult, _clamper.Clamp(positionToClamp));
        }
    }
}
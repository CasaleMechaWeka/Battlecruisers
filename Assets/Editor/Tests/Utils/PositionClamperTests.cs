using BattleCruisers.Utils.Clamping;
using BattleCruisers.Utils.DataStrctures;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Utils.Clamping
{
    public class PositionClamperTests
    {
		private IPositionClamper _clamper;
		private Rectangle _bounds;

        [SetUp]
        public void SetuUp()
        {
			_bounds = new Rectangle(-35, 35, 0, 30);
			_clamper = new PositionClamper(_bounds);
        }

		#region ClampVector2
		[Test]
        public void ClampVector2_WithinBounds()
        {
			Vector2 positionToClamp = new Vector2(0, 0);
			Assert.AreEqual(positionToClamp, _clamper.Clamp(positionToClamp));
        }

		[Test]
        public void ClampVector2_XTooSmall()
        {
			Vector2 positionToClamp = new Vector2(_bounds.MinX - 1, 0);
			Assert.AreEqual(new Vector2(_bounds.MinX, 0), _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector2_XTooBig()
        {
			Vector2 positionToClamp = new Vector2(_bounds.MaxX + 1, 0);
			Assert.AreEqual(new Vector2(_bounds.MaxX, 0), _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector2_YTooSmall()
        {
			Vector2 positionToClamp = new Vector2(0, _bounds.MinY - 1);
			Assert.AreEqual(new Vector2(0, _bounds.MinY), _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector2_YTooBig()
        {
			Vector2 positionToClamp = new Vector2(0, _bounds.MaxY + 1);
			Assert.AreEqual(new Vector2(0, _bounds.MaxY), _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector2_BothXAndYTooSmall()
        {
			Vector2 positionToClamp = new Vector2(_bounds.MinX - 1, _bounds.MinY - 1);
			Assert.AreEqual(new Vector2(_bounds.MinX, _bounds.MinY), _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector2_BothXAndYTooBig()
        {
			Vector2 positionToClamp = new Vector2(_bounds.MaxX + 1, _bounds.MaxY + 1);
			Assert.AreEqual(new Vector2(_bounds.MaxX, _bounds.MaxY), _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector2_XTooSmall_YTooBig()
        {
			Vector2 positionToClamp = new Vector2(_bounds.MinX - 1, _bounds.MaxY + 1);
			Assert.AreEqual(new Vector2(_bounds.MinX, _bounds.MaxY), _clamper.Clamp(positionToClamp));
        }

		[Test]
        public void ClampVector2_XTooBig_YTooSmall()
        {
			Vector2 positionToClamp = new Vector2(_bounds.MaxX + 1, _bounds.MinY - 1);
			Assert.AreEqual(new Vector2(_bounds.MaxX, _bounds.MinY), _clamper.Clamp(positionToClamp));
        }
		#endregion ClampVector2

		#region ClampVector3
        [Test]
        public void ClampVector3_WithinBounds()
        {
            Vector3 positionToClamp = new Vector3(0, 0, 17);
            Assert.AreEqual(positionToClamp, _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector3_XTooSmall()
        {
            Vector3 positionToClamp = new Vector3(_bounds.MinX - 1, 0, 17);
            Assert.AreEqual(new Vector3(_bounds.MinX, 0, 17), _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector3_XTooBig()
        {
            Vector3 positionToClamp = new Vector3(_bounds.MaxX + 1, 0, 17);
            Assert.AreEqual(new Vector3(_bounds.MaxX, 0, 17), _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector3_YTooSmall()
        {
            Vector3 positionToClamp = new Vector3(0, _bounds.MinY - 1, 17);
            Assert.AreEqual(new Vector3(0, _bounds.MinY, 17), _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector3_YTooBig()
        {
            Vector3 positionToClamp = new Vector3(0, _bounds.MaxY + 1, 17);
            Assert.AreEqual(new Vector3(0, _bounds.MaxY, 17), _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector3_BothXAndYTooSmall()
        {
            Vector3 positionToClamp = new Vector3(_bounds.MinX - 1, _bounds.MinY - 1, 17);
            Assert.AreEqual(new Vector3(_bounds.MinX, _bounds.MinY, 17), _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector3_BothXAndYTooBig()
        {
            Vector3 positionToClamp = new Vector3(_bounds.MaxX + 1, _bounds.MaxY + 1, 17);
            Assert.AreEqual(new Vector3(_bounds.MaxX, _bounds.MaxY, 17), _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector3_XTooSmall_YTooBig()
        {
            Vector3 positionToClamp = new Vector3(_bounds.MinX - 1, _bounds.MaxY + 1, 17);
            Assert.AreEqual(new Vector3(_bounds.MinX, _bounds.MaxY, 17), _clamper.Clamp(positionToClamp));
        }

        [Test]
        public void ClampVector3_XTooBig_YTooSmall()
        {
            Vector3 positionToClamp = new Vector3(_bounds.MaxX + 1, _bounds.MinY - 1, 17);
            Assert.AreEqual(new Vector3(_bounds.MaxX, _bounds.MinY, 17), _clamper.Clamp(positionToClamp));
        }
        #endregion ClampVector3
    }
}

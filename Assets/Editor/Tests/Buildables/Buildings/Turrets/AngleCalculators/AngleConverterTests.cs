using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleConverterTests
    {
        private IAngleConverter _angleConverter;

        [SetUp]
        public void TestSetup()
        {
            _angleConverter = new AngleConverter();
            UnityAsserts.Assert.raiseExceptions = true;
        }

        #region ConvertToSigned
        [Test]
        public void ConvertToSigned_TooSmall_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _angleConverter.ConvertToSigned(-1));
        }

        [Test]
        public void ConvertToSigned_TooLarge_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _angleConverter.ConvertToSigned(361));
        }

        [Test]
        public void ConvertToSigned_NoConversionNeeded()
        {
            float signedAngle = _angleConverter.ConvertToSigned(180);
            Assert.AreEqual(180, signedAngle);
        }

        [Test]
        public void ConvertToSigned_ConversionNeeded()
        {
            float signedAngle = _angleConverter.ConvertToSigned(181);
            Assert.AreEqual(-179, signedAngle);

        }
        #endregion ConvertToSigned

        #region ConvertToUnsigned
        [Test]
        public void ConvertToUnsigned_TooSmall_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _angleConverter.ConvertToUnsigned(-181));
        }

        [Test]
        public void ConvertToUnsigned_TooLarge_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _angleConverter.ConvertToUnsigned(181));
        }

        [Test]
        public void ConvertToUnsigned_NoConversionNeeded()
        {
            float signedAngle = _angleConverter.ConvertToUnsigned(0);
            Assert.AreEqual(0, signedAngle);
        }

        [Test]
        public void ConvertToUnsigned_ConversionNeeded()
        {
            float signedAngle = _angleConverter.ConvertToUnsigned(-90);
            Assert.AreEqual(270, signedAngle);

        }
        #endregion ConvertToUnsigned
    }
}
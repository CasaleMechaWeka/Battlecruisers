using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleConverterTests
    {

        [SetUp]
        public void TestSetup()
        {
        }

        #region ConvertToSigned
        [Test]
        public void ConvertToSigned_TooSmall_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => AngleConverter.ConvertToSigned(-1));
        }

        [Test]
        public void ConvertToSigned_TooLarge_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => AngleConverter.ConvertToSigned(361));
        }

        [Test]
        public void ConvertToSigned_NoConversionNeeded()
        {
            float signedAngle = AngleConverter.ConvertToSigned(180);
            Assert.AreEqual(180, signedAngle);
        }

        [Test]
        public void ConvertToSigned_ConversionNeeded()
        {
            float signedAngle = AngleConverter.ConvertToSigned(181);
            Assert.AreEqual(-179, signedAngle);

        }
        #endregion ConvertToSigned

        #region ConvertToUnsigned
        [Test]
        public void ConvertToUnsigned_TooSmall_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => AngleConverter.ConvertToUnsigned(-181));
        }

        [Test]
        public void ConvertToUnsigned_TooLarge_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => AngleConverter.ConvertToUnsigned(181));
        }

        [Test]
        public void ConvertToUnsigned_NoConversionNeeded()
        {
            float signedAngle = AngleConverter.ConvertToUnsigned(0);
            Assert.AreEqual(0, signedAngle);
        }

        [Test]
        public void ConvertToUnsigned_ConversionNeeded()
        {
            float signedAngle = AngleConverter.ConvertToUnsigned(-90);
            Assert.AreEqual(270, signedAngle);

        }
        #endregion ConvertToUnsigned
    }
}
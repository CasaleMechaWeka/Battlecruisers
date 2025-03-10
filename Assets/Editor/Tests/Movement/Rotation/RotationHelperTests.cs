using BattleCruisers.Movement.Rotation;
using NUnit.Framework;

namespace BattleCruisers.Tests.Movement.Rotation
{
    public class RotationHelperTests
    {
        private IRotationHelper _rotationHelper;

        [SetUp]
        public void TestSetup()
        {
            _rotationHelper = new RotationHelper();
        }

        [Test]
        public void FindDirectionMultiplier_OnTarget()
        {
            float currentAngleInDegrees = 0;
            float targetAngleInDegrees = 0;

            float directionMultiplier = _rotationHelper.FindDirectionMultiplier(currentAngleInDegrees, targetAngleInDegrees);
            Assert.AreEqual(0, directionMultiplier);
        }

        [Test]
        public void FindDirectionMultiplier_TtoS_LessThan180()
        {
            float currentAngleInDegrees = 0;
            float targetAngleInDegrees = 170;

            float directionMultiplier = _rotationHelper.FindDirectionMultiplier(currentAngleInDegrees, targetAngleInDegrees);
            Assert.AreEqual(1, directionMultiplier);
        }

        [Test]
        public void FindDirectionMultiplier_StoT_LessThan180()
        {
            float currentAngleInDegrees = 170;
            float targetAngleInDegrees = 0;

            float directionMultiplier = _rotationHelper.FindDirectionMultiplier(currentAngleInDegrees, targetAngleInDegrees);
            Assert.AreEqual(-1, directionMultiplier);
        }

        [Test]
        public void FindDirectionMultiplier_TtoS_MoreThan180()
        {
            float currentAngleInDegrees = 0;
            float targetAngleInDegrees = 190;

            float directionMultiplier = _rotationHelper.FindDirectionMultiplier(currentAngleInDegrees, targetAngleInDegrees);
            Assert.AreEqual(-1, directionMultiplier);
        }

        [Test]
        public void FindDirectionMultiplier_StoT_MoreThan180()
        {
            float currentAngleInDegrees = 190;
            float targetAngleInDegrees = 0;

            float directionMultiplier = _rotationHelper.FindDirectionMultiplier(currentAngleInDegrees, targetAngleInDegrees);
            Assert.AreEqual(1, directionMultiplier);
        }
    }
}
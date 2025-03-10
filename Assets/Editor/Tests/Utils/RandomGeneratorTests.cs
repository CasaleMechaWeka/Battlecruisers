using BattleCruisers.Utils;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils
{
    public class RandomGeneratorTests
    {
        private IRandomGenerator _randomGenerator;

        [SetUp]
        public void TestSetup()
        {
            _randomGenerator = RandomGenerator.Instance;
        }

        [Test]
        public void Randomise_BothDirections()
        {
            float baseValue = 5;
            float maxChangeByProportionOfBaseValue = 0.1f;
            ChangeDirection changeDirection = ChangeDirection.Both;
            float min = 4.5f;
            float max = 5.5f;

            ExpectWithinRange(baseValue, maxChangeByProportionOfBaseValue, changeDirection, min, max);
        }

        [Test]
        public void Randomise_Up()
        {
            float baseValue = 5;
            float maxChangeByProportionOfBaseValue = 0.1f;
            ChangeDirection changeDirection = ChangeDirection.Up;
            float min = 5;
            float max = 5.5f;

            ExpectWithinRange(baseValue, maxChangeByProportionOfBaseValue, changeDirection, min, max);
        }

        [Test]
        public void Randomise_Down()
        {
            float baseValue = 5;
            float maxChangeByProportionOfBaseValue = 0.1f;
            ChangeDirection changeDirection = ChangeDirection.Down;
            float min = 4.5f;
            float max = 5;

            ExpectWithinRange(baseValue, maxChangeByProportionOfBaseValue, changeDirection, min, max);
        }

        private void ExpectWithinRange(
            float baseValue,
            float maxChangeByProportionOfBaseValue,
            ChangeDirection changeDirection,
            float min,
            float max)
        {
            for (int i = 0; i < 100; i++)
            {
                float generatedValue = _randomGenerator.Randomise(baseValue, maxChangeByProportionOfBaseValue, changeDirection);
                Assert.IsTrue(generatedValue >= min);
                Assert.IsTrue(generatedValue <= max);
            }
        }
    }
}
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI
{
    public class StaticDeciderTests
    {
        [Test]
        public void ShouldBeEnabled_True()
        {
            IFilter<BuildingCategory> decider = new StaticFilter<BuildingCategory>(isMatch: true);
            Assert.IsTrue(decider.IsMatch(BuildingCategory.Ultra));
        }

        [Test]
        public void ShouldBeEnabled_False()
        {
            IFilter<BuildingCategory> decider = new StaticFilter<BuildingCategory>(isMatch: false);
            Assert.IsFalse(decider.IsMatch(BuildingCategory.Ultra));
        }
    }
}

using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI
{
    public class StaticFilterTests
    {
        [Test]
        public void ShouldBeEnabled_True()
        {
            IBroadcastingFilter<BuildingCategory> filter = new StaticFilter<BuildingCategory>(isMatch: true);
            Assert.IsTrue(filter.IsMatch(BuildingCategory.Ultra));
        }

        [Test]
        public void ShouldBeEnabled_False()
        {
            IBroadcastingFilter<BuildingCategory> filter = new StaticFilter<BuildingCategory>(isMatch: false);
            Assert.IsFalse(filter.IsMatch(BuildingCategory.Ultra));
        }
    }
}

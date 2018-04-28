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
            IActivenessDecider<BuildingCategory> decider = new StaticDecider<BuildingCategory>(shouldBeEnabled: true);
            Assert.IsTrue(decider.ShouldBeEnabled(BuildingCategory.Ultra));
        }

        [Test]
        public void ShouldBeEnabled_False()
        {
            IActivenessDecider<BuildingCategory> decider = new StaticDecider<BuildingCategory>(shouldBeEnabled: false);
            Assert.IsFalse(decider.ShouldBeEnabled(BuildingCategory.Ultra));
        }
    }
}

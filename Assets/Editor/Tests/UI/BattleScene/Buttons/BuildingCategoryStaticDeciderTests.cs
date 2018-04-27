using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons
{
    public class BuildingCategoryStaticDeciderTests
    {
        [Test]
        public void ShouldBeEnabled_True()
        {
            IActivenessDecider<BuildingCategory> decider = new BuildingCategoryStaticDecider(shouldBeEnabled: true);
            Assert.IsTrue(decider.ShouldBeEnabled(BuildingCategory.Ultra));
        }

        [Test]
        public void ShouldBeEnabled_False()
        {
            IActivenessDecider<BuildingCategory> decider = new BuildingCategoryStaticDecider(shouldBeEnabled: false);
            Assert.IsFalse(decider.ShouldBeEnabled(BuildingCategory.Ultra));
        }
    }
}

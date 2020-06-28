using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons.Filters
{
    // FELIX  fix :)
    public class BuildingCategoryFilterTests
    {
        private BuildingCategoryFilter _filter;

        [SetUp]
        public void SetuUp()
        {
            _filter = new BuildingCategoryFilter();
        }

        [Test]
        public void PermittedCategory_Set_TriggersPotentialMatchChange()
        {
            int eventCounter = 0;

            _filter.PotentialMatchChange += (sender, e) => eventCounter++;

//             _filter.PermittedCategory = null;

            Assert.AreEqual(1, eventCounter);
        }

        [Test]
        public void ShouldBeEnabled_False_NonePermitted()
        {
            Assert.IsFalse(_filter.IsMatch(BuildingCategory.Defence));
        }

        [Test]
        public void ShouldBeEnabled_False_WrongCategory()
        {
//             _filter.PermittedCategory = BuildingCategory.Ultra;
            Assert.IsFalse(_filter.IsMatch(BuildingCategory.Defence));
        }

        [Test]
        public void ShouldBeEnabled_True()
        {
//             _filter.PermittedCategory = BuildingCategory.Ultra;
            Assert.IsTrue(_filter.IsMatch(BuildingCategory.Ultra));
        }
    }
}

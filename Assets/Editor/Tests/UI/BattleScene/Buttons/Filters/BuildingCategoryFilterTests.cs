using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons.Filters
{
    public class BuildingCategoryFilterTests
    {
        private BuildingCategoryFilter _filter;
        private int _eventCounter;
        [SetUp]
        public void SetuUp()
        {
            _filter = new BuildingCategoryFilter();

            _eventCounter = 0;
            _filter.PotentialMatchChange += (sender, e) => _eventCounter++;
        }

        [Test]
        public void InitialState()
        {
            Assert.IsFalse(_filter.IsMatch(BuildingCategory.Defence));
        }

        [Test]
        public void AllowSingleCategory()
        {
            _filter.AllowSingleCategory(BuildingCategory.Defence);

            Assert.AreEqual(1, _eventCounter);
            Assert.IsTrue(_filter.IsMatch(BuildingCategory.Defence));
            Assert.IsFalse(_filter.IsMatch(BuildingCategory.Ultra));
        }

        [Test]
        public void AllowAllCategories()
        {
            _filter.AllowAllCategories();

            Assert.AreEqual(1, _eventCounter);
            Assert.IsTrue(_filter.IsMatch(BuildingCategory.Defence));
            Assert.IsTrue(_filter.IsMatch(BuildingCategory.Ultra));
        }

        [Test]
        public void AllowNoCategories()
        {
            _filter.AllowNoCategories();

            Assert.AreEqual(1, _eventCounter);
            Assert.IsFalse(_filter.IsMatch(BuildingCategory.Defence));
            Assert.IsFalse(_filter.IsMatch(BuildingCategory.Ultra));
        }
    }
}
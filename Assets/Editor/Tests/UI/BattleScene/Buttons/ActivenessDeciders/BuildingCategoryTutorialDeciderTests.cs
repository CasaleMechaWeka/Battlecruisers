using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons.ActivenessDeciders;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons.ActivenessDeciders
{
    public class BuildingCategoryTutorialDeciderTests
    {
        private BuildingCategoryTutorialDecider _decider;

        [SetUp]
        public void SetuUp()
        {
            _decider = new BuildingCategoryTutorialDecider();
        }

        [Test]
        public void PermittedCategory_Set_TriggersPotentialActivenessChange()
        {
            int eventCounter = 0;

            _decider.PotentialMatchChange += (sender, e) => eventCounter++;

            _decider.PermittedCategory = null;

            Assert.AreEqual(1, eventCounter);
        }

        [Test]
        public void ShouldBeEnabled_False_NonePermitted()
        {
            Assert.IsFalse(_decider.IsMatch(BuildingCategory.Defence));
        }

        [Test]
        public void ShouldBeEnabled_False_WrongCategory()
        {
            _decider.PermittedCategory = BuildingCategory.Ultra;
            Assert.IsFalse(_decider.IsMatch(BuildingCategory.Defence));
        }

        [Test]
        public void ShouldBeEnabled_True()
        {
            _decider.PermittedCategory = BuildingCategory.Ultra;
            Assert.IsTrue(_decider.IsMatch(BuildingCategory.Ultra));
        }
    }
}

using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons.Filters
{
    public class BuildingNameFilterTests
    {
        private BuildingNameFilter _filter;

        private IBuildable _buildableToFilter;
        private IPrefabKey _permittedBuildingKey;
        private IBuildableWrapper<IBuilding> _permittedBuildingWrapper;
        private IBuilding _permittedBuilding;
        private int _eventCounter;

        [SetUp]
        public void SetuUp()
        {
            _filter = new BuildingNameFilter();

            _eventCounter = 0;
            _filter.PotentialMatchChange += (sender, e) => _eventCounter++;

            _buildableToFilter = Substitute.For<IBuildable>();
            _buildableToFilter.Name.Returns("Zeit");
            _permittedBuildingKey = Substitute.For<IPrefabKey>();
            _permittedBuilding = Substitute.For<IBuilding>();

            _permittedBuildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            _permittedBuildingWrapper.Buildable.Returns(_permittedBuilding);
            PrefabFactory.GetBuildingWrapperPrefab(_permittedBuildingKey).Returns(_permittedBuildingWrapper);
        }

        [Test]
        public void PermittedBuilding_Set_Null()
        {
            _filter.PermittedBuilding = null;

            Assert.AreEqual(1, _eventCounter);
            PrefabFactory.GetBuildingWrapperPrefab(default);
        }

        [Test]
        public void PermittedBuilding_Set_BuildingKey()
        {
            _filter.PermittedBuilding = _permittedBuildingKey;

            Assert.AreEqual(1, _eventCounter);
            PrefabFactory.GetBuildingWrapperPrefab(_permittedBuildingKey);
        }

        [Test]
        public void ShouldBeEnabled_False_PermittedBuildingIsNull()
        {
            Assert.IsFalse(_filter.IsMatch(_buildableToFilter));
        }

        [Test]
        public void ShouldBeEnabled_False_NotPermittedBuilding()
        {
            _filter.PermittedBuilding = _permittedBuildingKey;
            _permittedBuilding.Name.Returns("Los");

            Assert.IsFalse(_filter.IsMatch(_buildableToFilter));
        }

        [Test]
        public void ShouldBeEnabled_True()
        {
            _filter.PermittedBuilding = _permittedBuildingKey;
            _permittedBuilding.Name.Returns("Zeit");

            Assert.IsTrue(_filter.IsMatch(_buildableToFilter));
        }
    }
}

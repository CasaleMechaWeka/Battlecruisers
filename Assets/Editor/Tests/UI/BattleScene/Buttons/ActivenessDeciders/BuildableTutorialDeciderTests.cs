using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.BattleScene.Buttons.ActivenessDeciders;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons.ActivenessDeciders
{
    public class BuildableTutorialDeciderTests
    {
        private BuildableTutorialDecider _decider;

        private IPrefabFactory _prefabFactory;
        private IBuildable _buildableToDecideOn;
        private IPrefabKey _permittedBuildingKey;
        private IBuildableWrapper<IBuilding> _permittedBuildingWrapper;
        private IBuilding _permittedBuilding;
        private int _eventCounter;

        [SetUp]
        public void SetuUp()
        {
            _prefabFactory = Substitute.For<IPrefabFactory>();
            _decider = new BuildableTutorialDecider(_prefabFactory);

            _eventCounter = 0;
			_decider.PotentialActivenessChange += (sender, e) => _eventCounter++;
            
            _buildableToDecideOn = Substitute.For<IBuildable>();
            _buildableToDecideOn.Name.Returns("Zeit");
            _permittedBuildingKey = Substitute.For<IPrefabKey>();
            _permittedBuilding = Substitute.For<IBuilding>();
			
            _permittedBuildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            _permittedBuildingWrapper.Buildable.Returns(_permittedBuilding);
			_prefabFactory.GetBuildingWrapperPrefab(_permittedBuildingKey).Returns(_permittedBuildingWrapper);
        }

        [Test]
        public void PermittedBuilding_Set_Null()
        {
            _decider.PermittedBuilding = null;

            Assert.AreEqual(1, _eventCounter);
            _prefabFactory.DidNotReceiveWithAnyArgs().GetBuildingWrapperPrefab(default(IPrefabKey));
        }

        [Test]
        public void PermittedBuilding_Set_BuildingKey()
        {
            _decider.PermittedBuilding = _permittedBuildingKey;

            Assert.AreEqual(1, _eventCounter);
            _prefabFactory.Received().GetBuildingWrapperPrefab(_permittedBuildingKey);
        }

        [Test]
        public void ShouldBeEnabled_False_PermittedBuildingIsNull()
        {
            Assert.IsFalse(_decider.ShouldBeEnabled(_buildableToDecideOn));
        }

        [Test]
        public void ShouldBeEnabled_False_NotPermittedBuilding()
        {
            _decider.PermittedBuilding = _permittedBuildingKey;
            _permittedBuilding.Name.Returns("Los");

            Assert.IsFalse(_decider.ShouldBeEnabled(_buildableToDecideOn));
        }

        [Test]
        public void ShouldBeEnabled_True()
        {
            _decider.PermittedBuilding = _permittedBuildingKey;
            _permittedBuilding.Name.Returns("Zeit");

            Assert.IsTrue(_decider.ShouldBeEnabled(_buildableToDecideOn));
        }
    }
}

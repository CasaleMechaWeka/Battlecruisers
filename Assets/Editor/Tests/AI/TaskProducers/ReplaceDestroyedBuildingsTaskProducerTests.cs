using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.TaskProducers
{
    public class ReplaceDestroyedBuildingsTaskProducerTests : TaskProducerTestsBase
    {
        private IBuildableWrapper<IBuilding> _buildingWrapper;
        private IBuilding _buildingWithKey, _buildingWithoutKey;
        private BuildingKey _buildingKey1;
        private IPrioritisedTask _buildingTask;
        private IList<BuildingKey> _unlockedBuildingKeys;

		[SetUp]
		public override void SetuUp()
		{
            base.SetuUp();

            _buildingWithKey = Substitute.For<IBuilding>();
            _buildingWithKey.Name.Returns("DeathstarLauncher");
            _buildingWithoutKey = Substitute.For<IBuilding>();
            _buildingWithoutKey.Name.Returns("SamSite");

            _buildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            _buildingWrapper.Buildable.Returns(_buildingWithKey);

            _buildingKey1 = new BuildingKey(BuildingCategory.Defence, "Kartoffeln");
            _buildingTask = Substitute.For<IPrioritisedTask>();
            _taskFactory.CreateConstructBuildingTask(TaskPriority.High, _buildingKey1).Returns(_buildingTask);
            
            _unlockedBuildingKeys = new List<BuildingKey>();
            _unlockedBuildingKeys.Add(_buildingKey1);
            _prefabFactory.GetBuildingWrapperPrefab(_buildingKey1).Returns(_buildingWrapper);

            new ReplaceDestroyedBuildingsTaskProducer(_tasks, _cruiser, _prefabFactory, _taskFactory, _unlockedBuildingKeys);

			UnityAsserts.Assert.raiseExceptions = true;
		}

		[Test]
		public void BuildingDestroyed_CreatesTask()
		{
            BuildingDestroyedEventArgs eventArgs = new BuildingDestroyedEventArgs(_buildingWithKey);
            _cruiser.BuildingDestroyed += Raise.EventWith(_cruiser, eventArgs);

			_tasks.Received().Add(_buildingTask);
		}

		[Test]
		public void BuildingDestroyed_NoPrefabKey_Throws()
		{
            BuildingDestroyedEventArgs eventArgs = new BuildingDestroyedEventArgs(_buildingWithoutKey);
            Assert.Throws<UnityAsserts.AssertionException>(() => _cruiser.BuildingDestroyed += Raise.EventWith(_cruiser, eventArgs));
		}
	}
}

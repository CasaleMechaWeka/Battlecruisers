using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Tests.Utils.Extensions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.TaskProducers
{
    public class ReplaceDestroyedBuildingsTaskProducerTests : TaskProducerTestsBase
    {
        private IBuilding _normalBuilding, _droneStation, _buildingWithoutKey;
        private IPrioritisedTask _normalRebuildTask, _highPriorityRebuildTask;

        [SetUp]
		public override void SetuUp()
        {
            base.SetuUp();

            // Building without key
            _buildingWithoutKey = Substitute.For<IBuilding>();
            _buildingWithoutKey.Name.Returns("SamSite");

            // Normal building
            _normalBuilding = Substitute.For<IBuilding>();
            BuildingKey normalBuildingKey = StaticPrefabKeys.Buildings.DeathstarLauncher;
            _normalRebuildTask = SetupBuilding(normalBuildingKey, _normalBuilding, "Deathstar", TaskPriority.Normal);

            // Drone station
            _droneStation = Substitute.For<IBuilding>();
            BuildingKey droneStationBuildingKey = StaticPrefabKeys.Buildings.DroneStation;
            _highPriorityRebuildTask = SetupBuilding(droneStationBuildingKey, _droneStation, "DroneStation", TaskPriority.High);

            IList<BuildingKey> unlockedBuildingKeys = new List<BuildingKey>()
            {
                normalBuildingKey,
                droneStationBuildingKey
            };

            new ReplaceDestroyedBuildingsTaskProducer(_tasks, _cruiser, _prefabFactory, _taskFactory, unlockedBuildingKeys);
        }

        private IPrioritisedTask SetupBuilding(
            BuildingKey buildingKey, 
            IBuilding building, 
            string buildingName, 
            TaskPriority taskPriority)
        {
            building.Name.Returns(buildingName);

            IBuildableWrapper<IBuilding> buildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            buildingWrapper.Buildable.Returns(building);
            _prefabFactory
                .GetBuildingWrapperPrefab(buildingKey)
                .Returns(buildingWrapper);

            IPrioritisedTask task = Substitute.For<IPrioritisedTask>();
            _taskFactory
                .CreateConstructBuildingTask(taskPriority, buildingKey)
                .Returns(task);

            return task;
        }

        [Test]
		public void BuildingDestroyed_NotDronesStation_CreatesNormalPriorityTask()
		{
            _cruiser.BuildingMonitor.EmitBuildingDestroyed(_normalBuilding);
			_tasks.Received().Add(_normalRebuildTask);
		}

        [Test]
        public void BuildingDestroyed_DronesStation_CreatesHighPriorityTask()
        {
            _cruiser.BuildingMonitor.EmitBuildingDestroyed(_droneStation);
            _tasks.Received().Add(_highPriorityRebuildTask);
        }

        [Test]
		public void BuildingDestroyed_NoPrefabKey_Throws()
		{
            Assert.Throws<UnityAsserts.AssertionException>(() => _cruiser.BuildingMonitor.EmitBuildingDestroyed(_buildingWithoutKey));
        }
    }
}

using System.Collections.Generic;
using BattleCruisers.AI;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI
{
    public class ReplaceDestroyedBuildingsTaskProducerTests
	{
		private ITaskList _tasks;
		private ICruiserController _cruiser;
		private ITaskFactory _taskFactory;
        private IBuilding _buildingWithKey, _buildingWithoutKey;
        private IPrefabKey _buildingKey1, _buildingKey2;
        private ITask _buildingTask;
        private IDictionary<IBuilding, IPrefabKey> _buildingToKey;

		[SetUp]
		public void SetuUp()
		{
            _tasks = Substitute.For<ITaskList>();
            _cruiser = Substitute.For<ICruiserController>();
            _taskFactory = Substitute.For<ITaskFactory>();

            _buildingWithKey = Substitute.For<IBuilding>();
            _buildingWithoutKey = Substitute.For<IBuilding>();

            _buildingKey1 = Substitute.For<IPrefabKey>();
            _buildingTask = Substitute.For<ITask>();
            _taskFactory.CreateConstructBuildingTask(TaskPriority.High, _buildingKey1).Returns(_buildingTask);
            
            _buildingKey2 = Substitute.For<IPrefabKey>();

            _buildingToKey = new Dictionary<IBuilding, IPrefabKey>();
            _buildingToKey.Add(_buildingWithKey, _buildingKey1);

			new ReplaceDestroyedBuildingsTaskProducer(_tasks, _cruiser, _taskFactory, _buildingToKey);

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

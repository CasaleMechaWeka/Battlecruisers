﻿using System.Collections.Generic;
using BattleCruisers.AI;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.TaskProducers
{
    public class BasicTaskProducerTests
	{
        private ITaskList _tasks;
        private ICruiserController _cruiser;
        private IPrefabFactory _prefabFactory;
        private ITaskFactory _taskFactory;
        private IList<IPrefabKey> _buildOrder;
        private IBuildableWrapper<IBuilding> _platformSlotBuildingWrapper, _deckSlotBuildingWrapper;
        private IBuilding _platformSlotBuilding, _deckSlotBuilding;
		private IPrefabKey _platformBuildingKey, _deckBuildingKey;
        private ITask _platformBuildingTask, _deckBuildingTask;

		[SetUp]
		public void SetuUp()
		{
			_tasks = Substitute.For<ITaskList>();
            _cruiser = Substitute.For<ICruiserController>();
            _prefabFactory = Substitute.For<IPrefabFactory>();
            _taskFactory = Substitute.For<ITaskFactory>();
            _buildOrder = new List<IPrefabKey>();

            _platformSlotBuilding = Substitute.For<IBuilding>();
            _platformSlotBuilding.SlotType.Returns(SlotType.Platform);
            _platformSlotBuildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            _platformSlotBuildingWrapper.Buildable.Returns(_platformSlotBuilding);
            _platformBuildingKey = Substitute.For<IPrefabKey>();
            _prefabFactory.GetBuildingWrapperPrefab(_platformBuildingKey).Returns(_platformSlotBuildingWrapper);
            _platformBuildingTask = Substitute.For<ITask>();
            _taskFactory.CreateConstructBuildingTask(TaskPriority.Normal, _platformBuildingKey).Returns(_platformBuildingTask);

            _deckSlotBuilding = Substitute.For<IBuilding>();
            _deckSlotBuilding.SlotType.Returns(SlotType.Deck);
            _deckSlotBuildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            _deckSlotBuildingWrapper.Buildable.Returns(_deckSlotBuilding);
            _deckBuildingKey = Substitute.For<IPrefabKey>();
            _prefabFactory.GetBuildingWrapperPrefab(_deckBuildingKey).Returns(_deckSlotBuildingWrapper);
            _deckBuildingTask = Substitute.For<ITask>();
            _taskFactory.CreateConstructBuildingTask(TaskPriority.Normal, _deckBuildingKey).Returns(_deckBuildingTask);
		}

		[Test]
		public void SingleKey_CreatesSingleTask()
        {
            _buildOrder.Add(_platformBuildingKey);

            _cruiser.GetSlotCount(SlotType.Platform).Returns(1);

            CreateTaskProducer();

            _tasks.Received().Add(_platformBuildingTask);
        }

		[Test]
		public void MultipleKeys_CreatesMultipleTask()
		{
			_buildOrder.Add(_platformBuildingKey);
            _buildOrder.Add(_deckBuildingKey);

			_cruiser.GetSlotCount(SlotType.Platform).Returns(1);
            _cruiser.GetSlotCount(SlotType.Deck).Returns(1);

			CreateTaskProducer();

			_tasks.Received().Add(_platformBuildingTask);
            _tasks.Received().Add(_deckBuildingTask);
		}

		[Test]
        public void SingleKey_CruiserDoesNotHaveCapacity_DoesNotCreateTask()
        {
            _buildOrder.Add(_platformBuildingKey);

			_cruiser.GetSlotCount(SlotType.Platform).Returns(0);

			CreateTaskProducer();

            _tasks.DidNotReceive().Add(_platformBuildingTask);
        }

		[Test]
		public void MultipleKeys_CruiserDoesNotHaveCapacity_OnlyCreatesTaskForKeysWithCapacity()
		{
			_buildOrder.Add(_platformBuildingKey);
			_buildOrder.Add(_deckBuildingKey);

			_cruiser.GetSlotCount(SlotType.Platform).Returns(1);
			_cruiser.GetSlotCount(SlotType.Deck).Returns(0);

			CreateTaskProducer();

			_tasks.Received().Add(_platformBuildingTask);
            _tasks.DidNotReceive().Add(_deckBuildingTask);
		}

        private void CreateTaskProducer()
        {
            new BasicTaskProducer(_tasks, _cruiser, _prefabFactory, _taskFactory, _buildOrder);
        }
    }
}

using System;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Tasks
{
    public class DeleteBuildingTaskTests
    {
        private ITask _task;
        private IBuilding _building;
        private int _numOfCompletedEvents;

        [SetUp]
        public void SetuUp()
        {
			_building = Substitute.For<IBuilding>();

            _task = new DeleteBuildingTask(_building);

            _task.Completed += _task_Completed;

            _numOfCompletedEvents = 0;
        }

        [Test]
        public void Start_InitiatesBuildingDelete()
        {
            _task.Start();
            _building.Received().InitiateDelete();
        }

        [Test]
        public void BuildingDestroyedEvent_CausesTaskCompletedEvent()
        {
            Start_InitiatesBuildingDelete();

            _building.Destroyed += Raise.EventWith(_building, new DestroyedEventArgs(_building));
            Assert.AreEqual(1, _numOfCompletedEvents);
        }

		private void _task_Completed(object sender, EventArgs e)
		{
			_numOfCompletedEvents++;
		}
	}
}

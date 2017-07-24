using System;
using BattleCruisers.AI;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI
{
    public class ConstructBuildingTaskTests
    {
        private ITask _task;

		private IPrefabKey _key;
		private IPrefabFactory _prefabFactory;
		private ICruiserController _cruiser;
        private IBuildableWrapper<Building> _prefab;
        private IBuildable _building;
        private ISlot _slot;

        [SetUp]
        public void SetuUp()
        {
            _key = Substitute.For<IPrefabKey>();
            _prefabFactory = Substitute.For<IPrefabFactory>();
            _cruiser = Substitute.For<ICruiserController>();

            _task = new ConstructBuildingTask(TaskPriority.High, _key, _prefabFactory, _cruiser);

            _prefab = Substitute.For<IBuildableWrapper<Building>>();
            _building = Substitute.For<IBuildable>();
            _slot = Substitute.For<ISlot>();
        }

        [Test]
        public void Start_StartsConstructingBuilding()
        {
            _prefab.Buildable.Returns(_building);

            _prefabFactory.GetBuildingWrapperPrefab(_key).Returns(_prefab);
            _cruiser.IsSlotAvailable(_building.SlotType).Returns(true);
            _cruiser.GetFreeSlot(_building.SlotType).Returns(_slot);
            _cruiser.ConstructBuilding(_prefab.UnityObject, _slot).Returns(_building);

            _task.Start();
        }
    }
}

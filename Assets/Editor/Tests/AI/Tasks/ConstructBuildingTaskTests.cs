using System;
using BattleCruisers.AI;
using BattleCruisers.AI.Tasks;
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

        [SetUp]
        public void SetuUp()
        {
            _key = Substitute.For<IPrefabKey>();
            _prefabFactory = Substitute.For<IPrefabFactory>();
            _cruiser = Substitute.For<ICruiserController>();

            _task = new ConstructBuildingTask(TaskPriority.High, _key, _prefabFactory, _cruiser);
        }

        [Test]
        public void Start_StartsConstructingBuilding()
        {
            
        }
    }
}

using System.Collections.Generic;
using BattleCruisers.AI;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI
{
    public class BasicTaskProducerTests
	{
        private ITaskList _tasks;
        private ICruiserController _cruiser;
        private IPrefabFactory _prefabFactory;
        private ITaskFactory _taskFactory;

		[SetUp]
		public void SetuUp()
		{
            IList<IPrefabKey> buildOrder;

			_tasks = Substitute.For<ITaskList>();
            _cruiser = Substitute.For<ICruiserController>();

            // FELIX  NEXT
		}

		[Test]
		public void SweetTest()
		{
		}
	}
}

using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI
{
    public class TaskConsumerTests
    {
        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;
        }

        #region NormalPriorityProvider.NewTaskProduced
        [Test]
        public void SweetTest()
        {

        }
		#endregion NormalPriorityProvider.NewTaskProduced
	}
}

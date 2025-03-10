using BattleCruisers.Data.Static.Strategies;
using BattleCruisers.Data.Static.Strategies.Requests;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Data.Static.Strategies
{
    public class StrategyTests
    {
        [Test]
        public void CopyConstructor()
        {
            IBaseStrategy baseStrategy = Substitute.For<IBaseStrategy>();
            OffensiveRequest offensiveRequest = new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
            {
                NumOfSlotsToUse = 1
            };
            IOffensiveRequest[] offensiveRequests = new OffensiveRequest[]
            {
                offensiveRequest
            };
            IStrategy originalStrategy = new Strategy(baseStrategy, offensiveRequests);
            IStrategy copiedStrategy = new Strategy(originalStrategy);

            // Strategies are originally equal
            Assert.AreEqual(originalStrategy, copiedStrategy);

            // Changes in the original strategy's requestes do not affect the copied strategy
            offensiveRequest.NumOfSlotsToUse++;
            Assert.AreNotEqual(originalStrategy, copiedStrategy);
        }
    }
}
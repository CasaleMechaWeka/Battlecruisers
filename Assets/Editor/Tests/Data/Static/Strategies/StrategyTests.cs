using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;
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
            IList<IPrefabKeyWrapper> baseStrategy = Substitute.For<IList<IPrefabKeyWrapper>>();
            OffensiveRequest offensiveRequest = new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
            {
                NumOfSlotsToUse = 1
            };
            OffensiveRequest[] offensiveRequests = new OffensiveRequest[]
            {
                offensiveRequest
            };
            Strategy originalStrategy = new Strategy(baseStrategy, offensiveRequests);
            Strategy copiedStrategy = new Strategy(originalStrategy);

            // Strategies are originally equal
            Assert.AreEqual(originalStrategy, copiedStrategy);

            // Changes in the original strategy's requestes do not affect the copied strategy
            offensiveRequest.NumOfSlotsToUse++;
            Assert.AreNotEqual(originalStrategy, copiedStrategy);
        }
    }
}
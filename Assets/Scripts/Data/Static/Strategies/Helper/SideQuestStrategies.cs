using BattleCruisers.Data.Static.Strategies.Requests;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Static.Strategies.Helper
{
    public class SideQuestStrategies : ILevelStrategies
    {
        private IList<IStrategy> _adaptiveStrategies;
        private IList<IStrategy> _basicStrategies;

        public SideQuestStrategies()
        {
            IList<IBaseStrategy> adaptiveBaseStrategies = CreateAdaptiveBaseStrategies();
            IList<IBaseStrategy> basicBaseStrategies = CreateBasicBaseStrategies();
            IList<IOffensiveRequest[]> offensiveRequests = CreateOffensiveRequests();

            _adaptiveStrategies = CreateStrategies(adaptiveBaseStrategies, offensiveRequests);
            _basicStrategies = CreateStrategies(basicBaseStrategies, offensiveRequests);
        }

        private IList<IBaseStrategy> CreateAdaptiveBaseStrategies()
        {
            return new List<IBaseStrategy>()
            {
                // Set 0: SideQuests 0 - 8
                new BalancedStrategy(),
                new BoomStrategy(),
                new RushStrategy(),
                new BoomStrategy(),
                new BalancedStrategy(),
                new BoomStrategy(),
                new BoomStrategy(),
                new RushStrategy(),
                new BalancedStrategy(),

                // Set 1: SideQuests 9 - ???
                new BoomStrategy(),
                new BoomStrategy(),
                new BalancedStrategy(),
                new BoomStrategy(),
                new BalancedStrategy()
            };
        }

        private IList<IBaseStrategy> CreateBasicBaseStrategies()
        {
            return new List<IBaseStrategy>()
            {
                // Set 0: SideQuests 0 - 8
                new BasicTurtleStrategy(),
                new BasicBoomDefensiveStrategy(),
                new BasicRushStrategy(),
                new BasicBoomAggressiveStrategy(),
                new BasicTurtleStrategy(),
                new BasicBoomAggressiveStrategy(),
                new BasicBoomDefensiveStrategy(),
                new BasicRushStrategy(),
                new BasicTurtleStrategy(),

                // Set 1: SideQuests 9 - ???
                new BasicBoomAggressiveStrategy(),
                new BasicBoomDefensiveStrategy(),
                new BasicBalancedStrategy(),
                new BasicTurtleStrategy(),
                new BasicBalancedStrategy()
            };
        }

        private IList<IOffensiveRequest[]> CreateOffensiveRequests()
        {
            return new List<IOffensiveRequest[]>()
            {
                // Set 0: SIdeQuests 0 - 8

                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },

                 new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },

                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },

                // Set 1: SideQuests 9 - ???
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },

                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },

                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },

                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High)
                },

                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High)
                }
            };
        }

        private IList<IStrategy> CreateStrategies(IList<IBaseStrategy> baseStrategies, IList<IOffensiveRequest[]> offensiveRequests)
        {
            Assert.AreEqual(baseStrategies.Count, offensiveRequests.Count);

            IList<IStrategy> strategies = new List<IStrategy>();

            for (int i = 0; i < baseStrategies.Count; ++i)
                strategies.Add(new Strategy(baseStrategies[i], offensiveRequests[i]));

            return strategies;
        }

        public IStrategy GetAdaptiveStrategy(int levelNum)
        {
            return GetStrategy(_adaptiveStrategies, levelNum);
        }

        public IStrategy GetBasicStrategy(int levelNum)
        {
            return GetStrategy(_basicStrategies, levelNum);
        }

        private IStrategy GetStrategy(IList<IStrategy> strategies, int levelNum)
        {
            int levelIndex = levelNum;
            Assert.IsTrue(levelIndex < strategies.Count);

            // Create a new object to avoid the same level reusing the
            // same strategy object.  This resulted in a subtle bug, as
            // a strategy's offensive requests would be modified.
            return new Strategy(strategies[levelIndex]);
        }
    }
}

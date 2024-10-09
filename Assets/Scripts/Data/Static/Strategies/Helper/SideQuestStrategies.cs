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
                // Set 1: SideQuests 0 - 8
                new BalancedStrategy(),
                new BoomStrategy(),
                new RushStrategy(),
                new BoomStrategy(),
                new BalancedStrategy(),
                new BoomStrategy(),
                new BoomStrategy(),
                new RushStrategy(),
                new BalancedStrategy(),

                // Set 2: SideQuests 9 - 23
                new BoomStrategy(),
                new BoomStrategy(),
                new BalancedStrategy(),
                new BoomStrategy(),
                new BalancedStrategy(),
                new BoomStrategy(),
                new BalancedStrategy(),
                new BoomStrategy(),
                new BoomStrategy(),
                new BalancedStrategy(),
                new BoomStrategy(),
                new BoomStrategy(),
                new BalancedStrategy(),
                new BoomStrategy(),
                new FortressPrimeStrategy(),

                // Set 3 for 6.5: SideQuests 24-30
                new BoomStrategy(),
                new BoomStrategy(),
                new BalancedStrategy(),
                new BoomStrategy(),
                new BalancedStrategy(),
                new BoomStrategy(),
                new BoomStrategy()
            };
        }

        private IList<IBaseStrategy> CreateBasicBaseStrategies()
        {
            return new List<IBaseStrategy>()
            {
                // Set 1: SideQuests 0 - 8
                new BasicTurtleStrategy(),
                new BasicBoomDefensiveStrategy(),
                new BasicRushStrategy(),
                new BasicBoomAggressiveStrategy(),
                new BasicTurtleStrategy(),
                new BasicBoomAggressiveStrategy(),
                new BasicBoomDefensiveStrategy(),
                new BasicRushStrategy(),
                new BasicTurtleStrategy(),

                // Set 2: SideQuests 9 - 23
                new BasicBoomAggressiveStrategy(),
                new BasicBoomDefensiveStrategy(),
                new BasicBalancedStrategy(),
                new BasicTurtleStrategy(),
                new BasicBalancedStrategy(),
                new BasicBoomAggressiveStrategy(),
                new BasicBalancedStrategy(),
                new BasicBoomDefensiveStrategy(),
                new BasicBOOMStrategy(),
                new BasicBalancedStrategy(),
                new BasicBoomAggressiveStrategy(),
                new BasicTurtleStrategy(),
                new BasicBalancedStrategy(),
                new BasicBOOMStrategy(),
                new BasicFortressPrimeStrategy(),

                 // Set 3 for 6.5: SideQuests 24-30
                new BasicBoomAggressiveStrategy(),
                new BasicBoomDefensiveStrategy(),
                new BasicBalancedStrategy(),
                new BasicTurtleStrategy(),
                new BasicBalancedStrategy(),
                new BasicBoomDefensiveStrategy(),
                new BasicBoomAggressiveStrategy(),
            };
        }

        private IList<IOffensiveRequest[]> CreateOffensiveRequests()
        {
            return new List<IOffensiveRequest[]>()
            {
                // Set 1: SideQuests 0 - 8

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

                // Set 2: SideQuests 9 - 23
                //SQ9
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                //SQ10
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },
                //SQ11
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                //SQ12
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High)
                },
                //SQ13
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High)
                },
                //SQ14
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                },
                //SQ15
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                },
                //SQ16
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                },
                //SQ17
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                },
                //SQ18
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                },
                //SQ19
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                },
                //SQ20
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                },
                //SQ21
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                },
                //SQ22
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                },
                //SQ23 Fortress Prime
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                },

                // Set 3 for 6.5 : SideQuests 24 - 30
                //SQ24
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                //SQ25
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                //SQ26
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                },
                //SQ27
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High)
                },
                //SQ28
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High)
                },
                //SQ29
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                },
                //SQ30
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },


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

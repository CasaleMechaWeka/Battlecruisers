﻿﻿﻿using System.Collections.Generic;
using BattleCruisers.Data.Static.Strategies;
using BattleCruisers.Data.Static.Strategies.Requests;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Static
{
    public class LevelStrategies : ILevelStrategies
    {
        private IList<IStrategy> _adaptiveStrategies;
        private IList<IStrategy> _basicStrategies;

        public LevelStrategies()
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
                // Set 1:  Levels 1 - 7
                new BalancedStrategy(),
				new BalancedStrategy(),
				new RushStrategy(),
				new BoomStrategy(),
				new BoomStrategy(),
				new RushStrategy(),
				new BalancedStrategy(),
                
                // Set 2:  Levels 8 - 14
                new BoomStrategy(),
				new RushStrategy(),
				new BalancedStrategy(),
				new BoomStrategy(),
				new RushStrategy(),
				new BoomStrategy(),
				new BalancedStrategy(),

                // Set 3:  Levels 15 - 21
				new BalancedStrategy(),
				new BoomStrategy(),
				new RushStrategy(),
				new BoomStrategy(),
				new BoomStrategy(),
				new RushStrategy(),
				new BalancedStrategy(),

                // Set 4:  Levels 22 - 28
                new BoomStrategy(),
                new BalancedStrategy(),
                new RushStrategy(),
                new BalancedStrategy(),
                new BoomStrategy(),
                new RushStrategy(),
                new BoomStrategy()
            };
        }

		private IList<IBaseStrategy> CreateBasicBaseStrategies()
		{
			return new List<IBaseStrategy>()
			{
                // Set 1:  Levels 1 - 7
                new BasicTurtleStrategy(),
				new BasicTurtleStrategy(),
                new BasicRushStrategy(),
                new BasicBoomDefensiveStrategy(),
                new BasicBoomAggressiveStrategy(),
				new BasicRushStrategy(),
                new BasicBalancedStrategy(),
				
				// Set 2:  Levels 8 - 14
                new BasicTurtleStrategy(),
                new BasicRushStrategy(),
                new BasicBalancedStrategy(),
                new BasicBoomAggressiveStrategy(),
                new BasicRushStrategy(),
                new BasicBoomAggressiveStrategy(),
                new BasicBalancedStrategy(),

                // Set 3:  Levels 15 - 21
				new BasicTurtleStrategy(),
				new BasicBoomAggressiveStrategy(),
				new BasicRushStrategy(),
				new BasicBoomDefensiveStrategy(),
				new BasicBoomAggressiveStrategy(),
				new BasicRushStrategy(),
				new BasicBalancedStrategy(),

                // Set 4: Levels 22 - 28
                new BasicBoomAggressiveStrategy(),
                new BasicBalancedStrategy(),
                new BasicRushStrategy(),
                new BasicTurtleStrategy(),
                new BasicBoomDefensiveStrategy(),
                new RushStrategy(),
                new BasicBoomAggressiveStrategy()
            };
		}

        private IList<IOffensiveRequest[]> CreateOffensiveRequests()
        {
            return new List<IOffensiveRequest[]>()
            {
                // Set 1:  Levels 1 - 7
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                
                // Set 2:  Levels 8 - 14
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
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
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },

                // Set 3:  Levels 15 - 21
                new IOffensiveRequest[]
                {
					new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
				new IOffensiveRequest[]
				{
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
				},
				new IOffensiveRequest[]
				{
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
				},
				new IOffensiveRequest[]
				{
					new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
					new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
				},
				new IOffensiveRequest[]
				{
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
					new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
				},
				new IOffensiveRequest[]
				{
					new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
					new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
				},
				new IOffensiveRequest[]
				{
					new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
					new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
				},

                // Set 4:  Levels 22 - 28
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
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
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                },
                new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                }
            };
        }

        private IList<IStrategy> CreateStrategies(IList<IBaseStrategy> baseStrategies, IList<IOffensiveRequest[]> offensiveRequests)
        {
            Assert.AreEqual(baseStrategies.Count, offensiveRequests.Count);

            IList<IStrategy> strategies = new List<IStrategy>();

            for (int i = 0; i < baseStrategies.Count; ++i)
            {
                strategies.Add(new Strategy(baseStrategies[i], offensiveRequests[i]));
            }

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
            int levelIndex = levelNum - 1;
            Assert.IsTrue(levelIndex < strategies.Count);

            // Create a new object to avoid the same level reusing the
            // same strategy object.  This resulted in a subtle bug, as
            // a strategy's offensive requests would be modified.
            return new Strategy(strategies[levelIndex]);
        }
    }
}

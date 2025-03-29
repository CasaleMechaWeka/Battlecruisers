using BattleCruisers.Data.Models.PrefabKeys.Wrappers;
using BattleCruisers.Data.Static.Strategies.Requests;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Static.Strategies.Helper
{
    public class LevelStrategies : ILevelStrategies
    {
        private IList<Strategy> _adaptiveStrategies;
        private IList<Strategy> _basicStrategies;

        public LevelStrategies()
        {
            IList<IList<IPrefabKeyWrapper>> adaptiveBaseStrategies = CreateAdaptiveBaseStrategies();
            IList<IList<IPrefabKeyWrapper>> basicBaseStrategies = CreateBasicBaseStrategies();
            IList<OffensiveRequest[]> offensiveRequests = CreateOffensiveRequests();

            _adaptiveStrategies = CreateStrategies(adaptiveBaseStrategies, offensiveRequests);
            _basicStrategies = CreateStrategies(basicBaseStrategies, offensiveRequests);
        }

        private IList<IList<IPrefabKeyWrapper>> CreateAdaptiveBaseStrategies()
        {
            return new List<IList<IPrefabKeyWrapper>>()
            {
                // Set 1:  Levels 1 - 3
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Rush,

                // Set 2:  Levels 4 - 7
				StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Rush,
                StaticBuildOrders.Adaptive.Balanced,
                
                // Set 3:  Levels 8 - 10
				StaticBuildOrders.Adaptive.Rush,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Rush,

                // Set 4:  Levels 11 - 14
				StaticBuildOrders.Adaptive.Rush,
                StaticBuildOrders.Adaptive.Rush,
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Balanced,

                //man o war
                StaticBuildOrders.Adaptive.Balanced,

                // Set 5:  Levels 15 - 17
				StaticBuildOrders.Adaptive.Rush,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Boom,

                // Set 6:  Levels 18 - 21
				StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Rush,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Balanced,

                // Set 7:  Levels 22 - 25
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Rush,
                StaticBuildOrders.Adaptive.Boom,


                // Set 8: Levels 27-31
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Rush,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Balanced,

                // Set 9: Levels 32-40
                /*new BasicTurtleStrategy()
                (IBaseStrategy)StaticBuildOrders.Basic.BoomDefensive,
                (IBaseStrategy)StaticBuildOrders.Basic.Rush,
                (IBaseStrategy)StaticBuildOrders.Basic.BoomAggressive,
                new BasicTurtleStrategy(),
                (IBaseStrategy)StaticBuildOrders.Basic.BoomAggressive,
                (IBaseStrategy)StaticBuildOrders.Basic.BoomDefensive,
                new RushStrategy(),
                new BasicTurtleStrategy()*/

                //Temp Set 9, Please change accordingly
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Rush,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Rush,
                StaticBuildOrders.Adaptive.Balanced
            };
        }

        private IList<IList<IPrefabKeyWrapper>> CreateBasicBaseStrategies()
        {
            return new List<IList<IPrefabKeyWrapper>>()
            {
                // Set 1:  Levels 1 - 3
                StaticBuildOrders.Basic.Turtle,
                StaticBuildOrders.Basic.Turtle,
                StaticBuildOrders.Basic.Rush,

                // Set 2:  Levels 4 - 7
                StaticBuildOrders.Basic.BoomDefensive,
                StaticBuildOrders.Basic.BoomAggressive,
                StaticBuildOrders.Basic.Rush,
                StaticBuildOrders.Basic.Balanced,
				
				// Set 3:  Levels 8 - 10
                StaticBuildOrders.Basic.Rush,
                StaticBuildOrders.Basic.Turtle,
                StaticBuildOrders.Basic.BoomAggressive,

                // Set 4:  Levels 11 - 14
                StaticBuildOrders.Basic.Rush,
                StaticBuildOrders.Basic.BoomAggressive,
                StaticBuildOrders.Basic.Balanced,
                StaticBuildOrders.Basic.Turtle,

                //man o war
                StaticBuildOrders.Basic.Turtle,

                // Set 5:  Levels 16 - 18
				StaticBuildOrders.Basic.Rush,
                StaticBuildOrders.Basic.BoomDefensive,
                StaticBuildOrders.Basic.BoomAggressive,

                // Set 6:  Levels 19 - 22
				StaticBuildOrders.Basic.Balanced,
                StaticBuildOrders.Basic.Rush,
                StaticBuildOrders.Basic.BoomAggressive,
                StaticBuildOrders.Basic.Balanced,

                // Set 7: Levels 23 - 26
                StaticBuildOrders.Basic.Turtle,
                StaticBuildOrders.Basic.BoomDefensive,
                StaticBuildOrders.Basic.Rush,
                StaticBuildOrders.Basic.BoomAggressive,

                 // Set 8: Levels 27-31
                StaticBuildOrders.Basic.Turtle,
                StaticBuildOrders.Basic.BoomDefensive,
                StaticBuildOrders.Basic.Rush,
                StaticBuildOrders.Basic.BoomAggressive,
                StaticBuildOrders.Basic.Turtle,

                // Set 9: Levels 32-40
                StaticBuildOrders.Basic.Turtle,
                StaticBuildOrders.Basic.BoomDefensive,
                StaticBuildOrders.Basic.Rush,
                StaticBuildOrders.Basic.BoomAggressive,
                StaticBuildOrders.Basic.Turtle,
                StaticBuildOrders.Basic.BoomAggressive,
                StaticBuildOrders.Basic.BoomDefensive,
                StaticBuildOrders.Basic.Rush,
                StaticBuildOrders.Basic.Turtle
            };
        }

        private IList<OffensiveRequest[]> CreateOffensiveRequests()
        {
            return new List<OffensiveRequest[]>()
            {
                // Set 1:  Levels 1 - 3
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low)
                },

                // Set 2:  Levels 4 - 7
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                
                // Set 3:  Levels 8 - 10
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low)
                },

                // Set 4:  Levels 11 - 14
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },

                //Man of war
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },

                // Set 5:  Levels 15 - 17
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },

                // Set 6:  Levels 18 - 21
				new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                },

                // Set 7:  Levels 22 - 25
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },

                //BCUMIE enemies
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                new OffensiveRequest[] //Huntress Prime
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },

                // Enemies for levels 32 - 40

                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },

                 new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },

                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                }
            };
        }

        private IList<Strategy> CreateStrategies(IList<IList<IPrefabKeyWrapper>> baseStrategies, IList<OffensiveRequest[]> offensiveRequests)
        {
            Assert.AreEqual(baseStrategies.Count, offensiveRequests.Count);

            IList<Strategy> strategies = new List<Strategy>();

            for (int i = 0; i < baseStrategies.Count; ++i)
                strategies.Add(new Strategy(baseStrategies[i], offensiveRequests[i]));

            return strategies;
        }

        public Strategy GetAdaptiveStrategy(int levelNum)
        {
            return GetStrategy(_adaptiveStrategies, levelNum);
        }

        public Strategy GetBasicStrategy(int levelNum)
        {
            return GetStrategy(_basicStrategies, levelNum);
        }

        private Strategy GetStrategy(IList<Strategy> strategies, int levelNum)
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

using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Requests;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Helper
{
    public class PvPLevelStrategies : IPvPLevelStrategies
    {
        private IList<IPvPStrategy> _adaptiveStrategies;
        private IList<IPvPStrategy> _basicStrategies;

        public PvPLevelStrategies()
        {
            IList<IPvPBaseStrategy> adaptiveBaseStrategies = CreateAdaptiveBaseStrategies();
            IList<IPvPBaseStrategy> basicBaseStrategies = CreateBasicBaseStrategies();
            IList<IPvPOffensiveRequest[]> offensiveRequests = CreateOffensiveRequests();

            _adaptiveStrategies = CreateStrategies(adaptiveBaseStrategies, offensiveRequests);
            _basicStrategies = CreateStrategies(basicBaseStrategies, offensiveRequests);
        }

        private IList<IPvPBaseStrategy> CreateAdaptiveBaseStrategies()
        {
            return new List<IPvPBaseStrategy>()
            {
                // Set 1:  Levels 1 - 3
                new PvPBalancedStrategy(),
                new PvPBalancedStrategy(),
                new PvPRushStrategy(),

                // Set 2:  Levels 4 - 7
				new PvPBoomStrategy(),
                new PvPBoomStrategy(),
                new PvPRushStrategy(),
                new PvPBalancedStrategy(),
                
                // Set 3:  Levels 8 - 10
				new PvPRushStrategy(),
                new PvPBoomStrategy(),
                new PvPBoomStrategy(),

                // Set 4:  Levels 11 - 14
				new PvPRushStrategy(),
                new PvPBoomStrategy(),
                new PvPBalancedStrategy(),
                new PvPBalancedStrategy(),

                //man o war
                new PvPBalancedStrategy(),

                // Set 5:  Levels 15 - 17
				new PvPRushStrategy(),
                new PvPBoomStrategy(),
                new PvPBoomStrategy(),

                // Set 6:  Levels 18 - 21
				new PvPBalancedStrategy(),
                new PvPRushStrategy(),
                new PvPBoomStrategy(),
                new PvPBalancedStrategy(),

                // Set 7:  Levels 22 - 25
                new PvPBalancedStrategy(),
                new PvPBoomStrategy(),
                new PvPRushStrategy(),
                new PvPBoomStrategy(),


                // Set 8: Levels 27-31
                new PvPBasicBOOMStrategy(),
                new PvPBasicBOOMStrategy(),
                new PvPRushStrategy(),
                new PvPBasicBOOMStrategy(),
                new PvPBalancedStrategy(),

                // Set 9: Levels 32-40
                /*new BasicTurtleStrategy()
                new BasicBoomDefensiveStrategy(),
                new BasicRushStrategy(),
                new BasicBoomAggressiveStrategy(),
                new BasicTurtleStrategy(),
                new BasicBoomAggressiveStrategy(),
                new BasicBoomDefensiveStrategy(),
                new RushStrategy(),
                new BasicTurtleStrategy()*/

                //Temp Set 9, Please change accordingly
                new PvPBalancedStrategy(),
                new PvPBoomStrategy(),
                new PvPRushStrategy(),
                new PvPBoomStrategy(),
                new PvPBalancedStrategy(),
                new PvPBoomStrategy(),
                new PvPBoomStrategy(),
                new PvPRushStrategy(),
                new PvPBalancedStrategy()
            };
        }

        private IList<IPvPBaseStrategy> CreateBasicBaseStrategies()
        {
            return new List<IPvPBaseStrategy>()
            {
                // Set 1:  Levels 1 - 3
                new PvPBasicTurtleStrategy(),
                new PvPBasicTurtleStrategy(),
                new PvPBasicRushStrategy(),

                // Set 2:  Levels 4 - 7
                new PvPBasicBoomDefensiveStrategy(),
                new PvPBasicBoomAggressiveStrategy(),
                new PvPBasicRushStrategy(),
                new PvPBasicBalancedStrategy(),
				
				// Set 3:  Levels 8 - 10
                new PvPBasicRushStrategy(),
                new PvPBasicTurtleStrategy(),
                new PvPBasicBoomAggressiveStrategy(),

                // Set 4:  Levels 11 - 14
                new PvPBasicRushStrategy(),
                new PvPBasicBoomAggressiveStrategy(),
                new PvPBasicBalancedStrategy(),
                new PvPBasicTurtleStrategy(),

                //man o war
               new PvPBasicTurtleStrategy(),

                // Set 5:  Levels 16 - 18
				new PvPBasicRushStrategy(),
                new PvPBasicBoomDefensiveStrategy(),
                new PvPBasicBoomAggressiveStrategy(),

                // Set 6:  Levels 19 - 22
				new PvPBasicBalancedStrategy(),
                new PvPBasicRushStrategy(),
                new PvPBasicBoomAggressiveStrategy(),
                new PvPBasicBalancedStrategy(),

                // Set 7: Levels 23 - 26
                new PvPBasicTurtleStrategy(),
                new PvPBasicBoomDefensiveStrategy(),
                new PvPBasicRushStrategy(),
                new PvPBasicBoomAggressiveStrategy(),

                 // Set 8: Levels 27-31
                new PvPBasicTurtleStrategy(),
                new PvPBasicBoomDefensiveStrategy(),
                new PvPBasicRushStrategy(),
                new PvPBasicBoomAggressiveStrategy(),
                new PvPBasicTurtleStrategy(),

                // Set 9: Levels 32-40
                new PvPBasicTurtleStrategy(),
                new PvPBasicBoomDefensiveStrategy(),
                new PvPBasicRushStrategy(),
                new PvPBasicBoomAggressiveStrategy(),
                new PvPBasicTurtleStrategy(),
                new PvPBasicBoomAggressiveStrategy(),
                new PvPBasicBoomDefensiveStrategy(),
                new PvPBasicRushStrategy(),
                new PvPBasicTurtleStrategy()
            };
        }

        private IList<IPvPOffensiveRequest[]> CreateOffensiveRequests()
        {
            return new List<IPvPOffensiveRequest[]>()
            {
                // Set 1:  Levels 1 - 3
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low)
                },

                // Set 2:  Levels 4 - 7
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.High)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.High)
                },
                
                // Set 3:  Levels 8 - 10
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.High)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low)
                },

                // Set 4:  Levels 11 - 14
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.High)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.High)
                },

                //Man of war
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low)
                },

                // Set 5:  Levels 15 - 17
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.High)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.High)
                },

                // Set 6:  Levels 18 - 21
				new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.High)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.High)
                },

                // Set 7:  Levels 22 - 25
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.High)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low)
                },

                //BCUMIE enemies
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.High)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[] //Huntress Prime
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low)
                },

                // Enemies for levels 32 - 40

                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.High)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low)
                },

                 new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.High)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low)
                },
                new IPvPOffensiveRequest[]
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Buildings, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Air, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low),
                    new PvPOffensiveRequest(PvPOffensiveType.Ultras, PvPOffensiveFocus.Low)
                },

                new IPvPOffensiveRequest[] //Broadsword
                {
                    new PvPOffensiveRequest(PvPOffensiveType.Naval, PvPOffensiveFocus.Low)
                }
            };
        }

        private IList<IPvPStrategy> CreateStrategies(IList<IPvPBaseStrategy> baseStrategies, IList<IPvPOffensiveRequest[]> offensiveRequests)
        {
            Assert.AreEqual(baseStrategies.Count, offensiveRequests.Count);

            IList<IPvPStrategy> strategies = new List<IPvPStrategy>();

            for (int i = 0; i < baseStrategies.Count; ++i)
            {
                strategies.Add(new PvPStrategy(baseStrategies[i], offensiveRequests[i]));
            }

            return strategies;
        }

        public IPvPStrategy GetAdaptiveStrategy(int levelNum)
        {
            return GetStrategy(_adaptiveStrategies, levelNum);
        }

        public IPvPStrategy GetBasicStrategy(int levelNum)
        {
            return GetStrategy(_basicStrategies, levelNum);
        }

        private IPvPStrategy GetStrategy(IList<IPvPStrategy> strategies, int levelNum)
        {
            int levelIndex = levelNum - 1;
            Assert.IsTrue(levelIndex < strategies.Count);

            // Create a new object to avoid the same level reusing the
            // same strategy object.  This resulted in a subtle bug, as
            // a strategy's offensive requests would be modified.
            return new PvPStrategy(strategies[levelIndex]);
        }
    }
}

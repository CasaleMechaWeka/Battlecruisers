using BattleCruisers.Data.Models.PrefabKeys.Wrappers;
using BattleCruisers.Data.Static.Strategies.Requests;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Static.Strategies.Helper
{
    public class SideQuestStrategies : ILevelStrategies
    {
        private IList<Strategy> _adaptiveStrategies;
        private IList<Strategy> _basicStrategies;

        public SideQuestStrategies()
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
                // Set 1: SideQuests 0 - 8
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Rush,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Rush,
                StaticBuildOrders.Adaptive.Balanced,

                // Set 2: SideQuests 9 - 23
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.FortressPrime,

                // Set 3 for 6.5: SideQuests 24-30
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Balanced,
                StaticBuildOrders.Adaptive.Boom,
                StaticBuildOrders.Adaptive.Boom
            };
        }

        private IList<IList<IPrefabKeyWrapper>> CreateBasicBaseStrategies()
        {
            return new List<IList<IPrefabKeyWrapper>>()
            {
                // Set 1: SideQuests 0 - 8
                StaticBuildOrders.Basic.Turtle,
                StaticBuildOrders.Basic.BoomDefensive,
                StaticBuildOrders.Basic.Rush,
                StaticBuildOrders.Basic.BoomAggressive,
                StaticBuildOrders.Basic.Turtle,
                StaticBuildOrders.Basic.BoomAggressive,
                StaticBuildOrders.Basic.BoomDefensive,
                StaticBuildOrders.Basic.Rush,
                StaticBuildOrders.Basic.Turtle,

                // Set 2: SideQuests 9 - 23
                StaticBuildOrders.Basic.BoomAggressive,
                StaticBuildOrders.Basic.BoomDefensive,
                StaticBuildOrders.Basic.Balanced,
                StaticBuildOrders.Basic.Turtle,
                StaticBuildOrders.Basic.Balanced,
                StaticBuildOrders.Basic.BoomAggressive,
                StaticBuildOrders.Basic.Balanced,
                StaticBuildOrders.Basic.BoomDefensive,
                StaticBuildOrders.Basic.BOOM,
                StaticBuildOrders.Basic.Balanced,
                StaticBuildOrders.Basic.BoomAggressive,
                StaticBuildOrders.Basic.Turtle,
                StaticBuildOrders.Basic.Balanced,
                StaticBuildOrders.Basic.BOOM,
                StaticBuildOrders.Basic.FortressPrime,

                 // Set 3 for 6.5: SideQuests 24-30
                StaticBuildOrders.Basic.BoomAggressive,
                StaticBuildOrders.Basic.BoomDefensive,
                StaticBuildOrders.Basic.Balanced,
                StaticBuildOrders.Basic.Turtle,
                StaticBuildOrders.Basic.Balanced,
                StaticBuildOrders.Basic.BoomDefensive,
                StaticBuildOrders.Basic.BoomAggressive,
            };
        }

        private IList<OffensiveRequest[]> CreateOffensiveRequests()
        {
            return new List<OffensiveRequest[]>()
            {
                // Set 1: SideQuests 0 - 8

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
                },

                // Set 2: SideQuests 9 - 23
                //SQ9
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                //SQ10
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },
                //SQ11
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                //SQ12
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High)
                },
                //SQ13
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High)
                },
                //SQ14
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                },
                //SQ15
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                },
                //SQ16
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                },
                //SQ17
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                },
                //SQ18
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                },
                //SQ19
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                },
                //SQ20
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                },
                //SQ21
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                },
                //SQ22
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                },
                //SQ23 Fortress Prime
                new OffensiveRequest[]
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
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                //SQ25
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                },
                //SQ26
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                },
                //SQ27
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High)
                },
                //SQ28
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High)
                },
                //SQ29
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                },
                //SQ30
                new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                },


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
            int levelIndex = levelNum;
            Assert.IsTrue(levelIndex < strategies.Count);

            // Create a new object to avoid the same level reusing the
            // same strategy object.  This resulted in a subtle bug, as
            // a strategy's offensive requests would be modified.
            return new Strategy(strategies[levelIndex]);
        }
    }
}

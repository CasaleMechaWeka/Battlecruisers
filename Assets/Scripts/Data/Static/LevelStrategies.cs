using System.Collections.Generic;
using BattleCruisers.AI.Providers.Strategies;
using BattleCruisers.AI.Providers.Strategies.Requests;

namespace BattleCruisers.Data.Static
{
    public class LevelStrategies : ILevelStrategies
    {
		public IList<IStrategy> AdaptiveStrategies { get; private set; }
		public IList<IStrategy> BasicStrategies { get; private set; }

        public LevelStrategies()
        {
            AdaptiveStrategies = CreateAdaptiveStrategies();
            BasicStrategies = CreateBasicStrategies();
        }

		private IList<IStrategy> CreateBasicStrategies()
        {
            return null;
        }

		private IList<IStrategy> CreateAdaptiveStrategies()
        {
            return new List<IStrategy>()
            {
                // Set 1:  Levels 1 - 7
                new Strategy(
                    new BalancedStrategy(), 
                    new BasicOffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)),
				new Strategy(
                    new BalancedStrategy(), 
                    new BasicOffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)),
				new Strategy(
                    new RushStrategy(),
					new BasicOffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new BasicOffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low)),
				new Strategy(
                    new BoomStrategy(),
                    new BasicOffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new BasicOffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)),
				new Strategy(
                    new BoomStrategy(),
					new BasicOffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new BasicOffensiveRequest(OffensiveType.Air, OffensiveFocus.High)),
				new Strategy(
                    new RushStrategy(),
					new BasicOffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
					new BasicOffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new BasicOffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)),
				new Strategy(
					new BalancedStrategy(),
                    new BasicOffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
					new BasicOffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)),
                
                // Set 2:  Levels 8 - 14
                new Strategy(
                    new BoomStrategy(),
                    new BasicOffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)),
				new Strategy(
                    new RushStrategy(),
                    new BasicOffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new BasicOffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new BasicOffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low)),
				new Strategy(
                    new BalancedStrategy(),
					new BasicOffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new BasicOffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)),
				new Strategy(
                    new BoomStrategy(),
					new BasicOffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
					new BasicOffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
					new BasicOffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new BasicOffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)),
				new Strategy(
					new RushStrategy(),
                    new BasicOffensiveRequest(OffensiveType.Air, OffensiveFocus.High)),
				new Strategy(
                    new BoomStrategy(),
                    new BasicOffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new BasicOffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)),
				new Strategy(
                    new BalancedStrategy(),
					new BasicOffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
					new BasicOffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new BasicOffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new BasicOffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)),

                // Set 3:  Levels 15 - 21
                // FELIX
            };
        }
    }
}
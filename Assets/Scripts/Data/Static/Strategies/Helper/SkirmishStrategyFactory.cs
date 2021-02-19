using BattleCruisers.Data.Static.Strategies.Requests;

namespace BattleCruisers.Data.Static.Strategies.Helper
{
    public enum StrategyType
    {
        Balanced, Rush, Boom
    }

    public class SkirmishStrategyFactory : IStrategyFactory
    {
        private readonly StrategyType _strategyType;
        private readonly IOffensiveRequest[] _offensiveRequests;

        public SkirmishStrategyFactory(StrategyType strategyType)
        {
            _strategyType = strategyType;
            
            // FELIX  Implement properly :P
            _offensiveRequests
                = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
        }

        // FELIX  Implement properly :P
        public IStrategy GetAdaptiveStrategy()
        {
            return
                new Strategy(
                    new BalancedStrategy(),
                    _offensiveRequests);
        }

        // FELIX  Implement properly :P
        public IStrategy GetBasicStrategy()
        {
            return
                new Strategy(
                    new BasicBalancedStrategy(),
                    _offensiveRequests);
        }
    }
}
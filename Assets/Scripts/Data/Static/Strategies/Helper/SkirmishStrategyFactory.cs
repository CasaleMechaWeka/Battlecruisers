using BattleCruisers.Data.Static.Strategies.Requests;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Data.Static.Strategies.Helper
{
    public enum StrategyType
    {
        Balanced, Rush, Boom
    }

    public class SkirmishStrategyFactory : IStrategyFactory
    {
        private readonly StrategyType _strategyType;
        private readonly IRandomGenerator _random;
        private readonly IOffensiveRequest[] _offensiveRequests;

        public SkirmishStrategyFactory(StrategyType strategyType)
        {
            _strategyType = strategyType;
            _random = RandomGenerator.Instance;
            
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
            // FELIX  For boom, randomly choose between defensive and aggressive
            return
                new Strategy(
                    new BasicBalancedStrategy(),
                    _offensiveRequests);
        }

        // FELIX
        //private IBaseStrategy GetBaseStrategy(StrategyType strategyType)

        private IOffensiveRequest[] GetOffensiveRequests(StrategyType strategyType)
        {
            IList<IOffensiveRequest[]> allOptions = GetOffensiveRequestsList(strategyType);
            return _random.RandomItem(allOptions); 
        }

        private IList<IOffensiveRequest[]> GetOffensiveRequestsList(StrategyType strategyType)
        {
            switch (strategyType)
            {
                case StrategyType.Rush:
                    return OffensiveRequestsProvider.Rush.All;

                case StrategyType.Balanced:
                    return OffensiveRequestsProvider.Balanced.All;

                case StrategyType.Boom:
                    return OffensiveRequestsProvider.Boom.All;

                default:
                    throw new InvalidOperationException($"Unknown strategy type: {strategyType}");
            }
        }
    }
}
using BattleCruisers.Data.Static.Strategies.Requests;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Data.Static.Strategies.Helper
{
    public class SkirmishStrategyFactory : IStrategyFactory
    {
        private readonly StrategyType _strategyType;
        private readonly bool _canUseUltras;

        public SkirmishStrategyFactory(StrategyType strategyType, bool canUseUltras)
        {
            _strategyType = strategyType;
            _canUseUltras = canUseUltras;
        }

        public Strategy GetAdaptiveStrategy()
        {
            return
                new Strategy(
                    GetAdaptiveBaseStrategy(_strategyType),
                    GetOffensiveRequests(_strategyType));
        }

        private IBaseStrategy GetAdaptiveBaseStrategy(StrategyType strategyType)
        {
            switch (strategyType)
            {
                case StrategyType.Rush:
                    return new RushStrategy();

                case StrategyType.Balanced:
                    return new BalancedStrategy();

                case StrategyType.Boom:
                    return new BoomStrategy();

                default:
                    throw new InvalidOperationException($"Unknown strategy type: {strategyType}");
            }
        }

        public Strategy GetBasicStrategy()
        {
            return
                new Strategy(
                    GetBasicBaseStrategy(_strategyType),
                    GetOffensiveRequests(_strategyType));
        }

        private IBaseStrategy GetBasicBaseStrategy(StrategyType strategyType)
        {
            switch (strategyType)
            {
                case StrategyType.Rush:
                    return new BasicRushStrategy();

                case StrategyType.Balanced:
                    return new BasicBalancedStrategy();

                case StrategyType.Boom:
                    if (RandomGenerator.NextBool())
                    {
                        return new BasicBoomAggressiveStrategy();
                    }
                    else
                    {
                        return new BasicBoomDefensiveStrategy();
                    }

                default:
                    throw new InvalidOperationException($"Unknown strategy type: {strategyType}");
            }
        }

        private OffensiveRequest[] GetOffensiveRequests(StrategyType strategyType)
        {
            IList<OffensiveRequest[]> allOptions;

            if (_canUseUltras)
            {
                allOptions = GetOffensiveRequestsList(strategyType);
            }
            else
            {
                allOptions = GetOffensiveRequestsListNoUltras(strategyType);
            }

            return RandomGenerator.RandomItem(allOptions);
        }

        private IList<OffensiveRequest[]> GetOffensiveRequestsList(StrategyType strategyType)
        {
            return strategyType switch
            {
                StrategyType.Rush => OffensiveRequestsProvider.Rush.All,
                StrategyType.Balanced => OffensiveRequestsProvider.Balanced.All,
                StrategyType.Boom => OffensiveRequestsProvider.Boom.All,
                _ => throw new InvalidOperationException($"Unknown strategy type: {strategyType}"),
            };

        }

        private IList<OffensiveRequest[]> GetOffensiveRequestsListNoUltras(StrategyType strategyType)
        {
            return strategyType switch
            {
                StrategyType.Rush => OffensiveRequestsProvider.Rush.NoUltras,
                StrategyType.Balanced => OffensiveRequestsProvider.Balanced.NoUltras,
                StrategyType.Boom => OffensiveRequestsProvider.Boom.NoUltras,
                _ => throw new InvalidOperationException($"Unknown strategy type: {strategyType}"),
            };

        }
    }
}
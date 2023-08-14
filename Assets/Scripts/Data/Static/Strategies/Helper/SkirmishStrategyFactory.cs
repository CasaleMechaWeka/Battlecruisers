using BattleCruisers.Data.Static.Strategies.Requests;
using BattleCruisers.Utils;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Data.Static.Strategies.Helper
{
    public class SkirmishStrategyFactory : IStrategyFactory
    {
        private readonly StrategyType _strategyType;
        private readonly bool _canUseUltras;
        private readonly IRandomGenerator _random;

        public SkirmishStrategyFactory(StrategyType strategyType, bool canUseUltras)
        {
            _strategyType = strategyType;
            _canUseUltras = canUseUltras;
            _random = RandomGenerator.Instance;
        }

        public IStrategy GetAdaptiveStrategy()
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

        public IStrategy GetBasicStrategy()
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
                    if (_random.NextBool())
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

        private IOffensiveRequest[] GetOffensiveRequests(StrategyType strategyType)
        {
            IList<IOffensiveRequest[]> allOptions;

            if (_canUseUltras)
            {
                allOptions = GetOffensiveRequestsList(strategyType);
            }
            else
            {
                allOptions = GetOffensiveRequestsListNoUltras(strategyType);
            }

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

        private IList<IOffensiveRequest[]> GetOffensiveRequestsListNoUltras(StrategyType strategyType)
        {
            switch (strategyType)
            {
                case StrategyType.Rush:
                    return OffensiveRequestsProvider.Rush.NoUltras;

                case StrategyType.Balanced:
                    return OffensiveRequestsProvider.Balanced.NoUltras;

                case StrategyType.Boom:
                    return OffensiveRequestsProvider.Boom.NoUltras;

                default:
                    throw new InvalidOperationException($"Unknown strategy type: {strategyType}");
            }
        }
    }
}
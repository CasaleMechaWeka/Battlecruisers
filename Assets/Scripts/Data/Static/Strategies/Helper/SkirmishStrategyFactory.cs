using BattleCruisers.Data.Models.PrefabKeys.Wrappers;
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

        private IList<IPrefabKeyWrapper> GetAdaptiveBaseStrategy(StrategyType strategyType)
        {
            return strategyType switch
            {
                StrategyType.Rush => StaticBuildOrders.Adaptive.Rush,
                StrategyType.Balanced => StaticBuildOrders.Adaptive.Balanced,
                StrategyType.Boom => StaticBuildOrders.Adaptive.Boom,
                _ => throw new InvalidOperationException($"Unknown strategy type: {strategyType}"),
            };

        }

        public Strategy GetBasicStrategy()
        {
            return
                new Strategy(
                    GetBasicBaseStrategy(_strategyType),
                    GetOffensiveRequests(_strategyType));
        }

        private IList<IPrefabKeyWrapper> GetBasicBaseStrategy(StrategyType strategyType)
        {
            switch (strategyType)
            {
                case StrategyType.Rush:
                    return StaticBuildOrders.Basic.Rush;

                case StrategyType.Balanced:
                    return StaticBuildOrders.Basic.Balanced;

                case StrategyType.Boom:
                    if (RandomGenerator.NextBool())
                    {
                        return StaticBuildOrders.Basic.BoomAggressive;
                    }
                    else
                    {
                        return StaticBuildOrders.Basic.BoomDefensive;
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
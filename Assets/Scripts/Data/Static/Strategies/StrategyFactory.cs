using BattleCruisers.Data.Static.Strategies.Requests;

namespace BattleCruisers.Data.Static.Strategies
{
    public class StrategyFactory : IStrategyFactory
    {
        public IStrategy CreateStrategy(bool isAdaptiveAI, StrategyType strategyType)
        {
            // FELIX  Implement properly :P
            IOffensiveRequest[] offensiveRequests
                = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };

            if (isAdaptiveAI)
            {
                return
                    new Strategy(
                        new BalancedStrategy(),
                        offensiveRequests);
            }
            else
            {
                return
                    new Strategy(
                        new BasicBalancedStrategy(),
                        offensiveRequests);
            }
        }
    }
}
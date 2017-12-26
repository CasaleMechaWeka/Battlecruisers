namespace BattleCruisers.Targets.TargetProcessors.Ranking.Wrappers
{
    public class ShipTargetRankerWrapper : ITargetRankerWrapper
    {
        public ITargetRanker CreateTargetRanker(ITargetsFactory targetsFactory)
        {
            return targetsFactory.CreateShipTargetRanker();
        }
    }
}

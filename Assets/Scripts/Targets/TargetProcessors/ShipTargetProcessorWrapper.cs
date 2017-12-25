using BattleCruisers.Targets.TargetProcessors.Ranking;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class ShipTargetProcessorWrapper : ProximityTargetProcessorWrapper
	{
        protected override ITargetRanker CreateTargetRanker(ITargetsFactory targetsFactory)
        {
            return targetsFactory.CreateShipTargetRanker();
        }
    }
}

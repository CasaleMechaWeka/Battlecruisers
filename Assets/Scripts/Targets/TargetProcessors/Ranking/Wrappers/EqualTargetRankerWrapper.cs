namespace BattleCruisers.Targets.TargetProcessors.Ranking.Wrappers
{
    public class EqualTargetRankerWrapper : ITargetRankerWrapper
    {
        public ITargetRanker CreateTargetRanker(ITargetsFactory targetsFactory)
        {
            return targetsFactory.CreateEqualTargetRanker();
        }
    }
}

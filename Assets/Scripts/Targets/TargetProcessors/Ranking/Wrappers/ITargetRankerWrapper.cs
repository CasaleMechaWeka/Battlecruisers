namespace BattleCruisers.Targets.TargetProcessors.Ranking.Wrappers
{
    public interface ITargetRankerWrapper
    {
        ITargetRanker CreateTargetRanker(ITargetsFactory targetsFactory);
    }
}

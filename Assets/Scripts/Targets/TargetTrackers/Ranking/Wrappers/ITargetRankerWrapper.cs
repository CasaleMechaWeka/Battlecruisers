namespace BattleCruisers.Targets.TargetTrackers.Ranking.Wrappers
{
    public interface ITargetRankerWrapper
    {
        ITargetRanker CreateTargetRanker(ITargetsFactory targetsFactory);
    }
}

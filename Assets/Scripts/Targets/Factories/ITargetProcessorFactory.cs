using BattleCruisers.Targets.TargetProcessors;

namespace BattleCruisers.Targets.Factories
{
    public interface ITargetProcessorFactory
    {
        ITargetProcessor BomberTargetProcessor { get; }
        ITargetProcessor OffensiveBuildableTargetProcessor { get; }
        ITargetProcessor StaticTargetProcessor { get; }
    }
}
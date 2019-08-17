using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class TargetFactories
    {
        public ITargetFactoriesProvider TargetFactoriesProvider { get; }

        // Cruiser specific
        public ITargetProcessorFactory TargetProcessorFactory { get; }
        public ITargetTrackerFactory TargetTrackerFactory { get; } 
        public ITargetDetectorFactory TargetDetectorFactory { get; }
    }
}
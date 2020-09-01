using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Buildables.Pools
{
    public class UnitActivationArgs : BuildableActivationArgs
    {
        public IFactory ParentFactory { get; }

        public UnitActivationArgs(
            ICruiser parentCruiser,
            ICruiser enemyCruiser,
            ICruiserSpecificFactories cruiserSpecificFactories,
            IFactory parentFactory)
            : base(parentCruiser, enemyCruiser, cruiserSpecificFactories)
        {
            // May be null, not all units are created by a factory (spy satellite, deathstar)
            ParentFactory = parentFactory;
        }
    }
}
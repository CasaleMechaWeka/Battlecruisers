using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Buildables.Pools
{
    public class BuildableActivationArgs
    {
        public ICruiser ParentCruiser { get; }
        public ICruiser EnemyCruiser { get; }
        public CruiserSpecificFactories CruiserSpecificFactories { get; }

        public BuildableActivationArgs(
            ICruiser parentCruiser,
            ICruiser enemyCruiser,
            CruiserSpecificFactories cruiserSpecificFactories)
        {
            Helper.AssertIsNotNull(parentCruiser, enemyCruiser, cruiserSpecificFactories);

            ParentCruiser = parentCruiser;
            EnemyCruiser = enemyCruiser;
            CruiserSpecificFactories = cruiserSpecificFactories;
        }
    }
}
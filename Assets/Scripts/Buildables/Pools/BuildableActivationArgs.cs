using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Buildables.Pools
{
    public class BuildableActivationArgs
    {
        public ICruiser ParentCruiser { get; }
        public ICruiser EnemyCruiser { get; }
        public ICruiserSpecificFactories CruiserSpecificFactories { get; }

        public BuildableActivationArgs(
            ICruiser parentCruiser,
            ICruiser enemyCruiser,
            ICruiserSpecificFactories cruiserSpecificFactories)
        {
            Helper.AssertIsNotNull(parentCruiser, enemyCruiser, cruiserSpecificFactories);

            ParentCruiser = parentCruiser;
            EnemyCruiser = enemyCruiser;
            CruiserSpecificFactories = cruiserSpecificFactories;
        }
    }
}
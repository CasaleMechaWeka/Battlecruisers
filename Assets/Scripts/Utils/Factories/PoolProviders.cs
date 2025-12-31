using BattleCruisers.Buildables.Pools;

namespace BattleCruisers.Utils.Factories
{
    public class PoolProviders
    {
        private UnitPoolProvider _unitPoolProvider;
        public UnitPoolProvider UnitPoolProvider => _unitPoolProvider;

        public UnitToPoolMap UnitToPoolMap { get; }

        public PoolProviders()
        {
            _unitPoolProvider = new UnitPoolProvider();

            UnitToPoolMap = new UnitToPoolMap(UnitPoolProvider);
        }
    }
}
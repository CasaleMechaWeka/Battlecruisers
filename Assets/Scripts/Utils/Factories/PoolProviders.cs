using BattleCruisers.Buildables.Pools;
using BattleCruisers.UI.BattleScene.Manager;

namespace BattleCruisers.Utils.Factories
{
    public class PoolProviders
    {
        private UnitPoolProvider _unitPoolProvider;
        public UnitPoolProvider UnitPoolProvider => _unitPoolProvider;

        public UnitToPoolMap UnitToPoolMap { get; }

        public PoolProviders(UIManager uiManager)
        {
            Helper.AssertIsNotNull(uiManager);

            _unitPoolProvider = new UnitPoolProvider(uiManager);

            UnitToPoolMap = new UnitToPoolMap(UnitPoolProvider);
        }
    }
}
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public class PvPUnitFactory : IPoolableFactory<PvPUnit, PvPBuildableActivationArgs>
    {
        private readonly IPrefabKey _unitKey;
        // private readonly IPvPUIManager _uiManager;
        private readonly IPvPBuildableWrapper<IPvPUnit> _unitPrefab;

        public PvPUnitFactory(IPrefabKey unitKey /* IPvPUIManager uiManager, */)
        {
            PvPHelper.AssertIsNotNull(unitKey);

            _unitKey = unitKey;
            // _uiManager = uiManager;

            _unitPrefab = PvPPrefabFactory.GetUnitWrapperPrefab(unitKey);
        }

        public PvPUnit CreateItem()
        {
            var unit = PvPPrefabFactory
                    .CreateUnit(_unitPrefab /* _uiManager */ );
            return unit.Parse<PvPUnit>();
            /*        _prefabFactory
                        .CreateUnit(_unitPrefab, *//* _uiManager *//* _factoryProvider)
                        .Parse<PvPUnit>();*/
        }

        public override string ToString()
        {
            return $"{nameof(PvPUnitFactory)} {_unitPrefab.Buildable}";
        }
    }
}
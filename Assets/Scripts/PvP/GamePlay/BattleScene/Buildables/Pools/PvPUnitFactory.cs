using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public class PvPUnitFactory : IPoolableFactory<PvPUnit, PvPBuildableActivationArgs>
    {
        private readonly PvPPrefabFactory _prefabFactory;
        private readonly IPrefabKey _unitKey;
        // private readonly IPvPUIManager _uiManager;
        private readonly IPvPFactoryProvider _factoryProvider;
        private readonly IPvPBuildableWrapper<IPvPUnit> _unitPrefab;

        public PvPUnitFactory(PvPPrefabFactory prefabFactory, IPrefabKey unitKey, /* IPvPUIManager uiManager, */IPvPFactoryProvider factoryProvider)
        {
            PvPHelper.AssertIsNotNull(prefabFactory, unitKey, factoryProvider);

            _prefabFactory = prefabFactory;
            _unitKey = unitKey;
            // _uiManager = uiManager;
            _factoryProvider = factoryProvider;

            _unitPrefab = prefabFactory.GetUnitWrapperPrefab(unitKey);
        }

        public PvPUnit CreateItem()
        {
            var unit = _prefabFactory
                    .CreateUnit(_unitPrefab, /* _uiManager */ _factoryProvider);
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
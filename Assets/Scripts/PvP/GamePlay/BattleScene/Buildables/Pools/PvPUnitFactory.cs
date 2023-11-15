using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System.Threading.Tasks;
//using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public class PvPUnitFactory : IPvPPoolableFactory<PvPUnit, PvPBuildableActivationArgs>
    {
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IPvPPrefabKey _unitKey;
        // private readonly IPvPUIManager _uiManager;
        private readonly IPvPFactoryProvider _factoryProvider;
        private readonly IPvPBuildableWrapper<IPvPUnit> _unitPrefab;

        public PvPUnitFactory(IPvPPrefabFactory prefabFactory, IPvPPrefabKey unitKey, /* IPvPUIManager uiManager, */IPvPFactoryProvider factoryProvider)
        {
            PvPHelper.AssertIsNotNull(prefabFactory, unitKey, factoryProvider);

            _prefabFactory = prefabFactory;
            _unitKey = unitKey;
            // _uiManager = uiManager;
            _factoryProvider = factoryProvider;

            _unitPrefab = prefabFactory.GetUnitWrapperPrefab(unitKey);
        }

        public async Task<PvPUnit> CreateItem()
        {
            var unit = await _prefabFactory
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
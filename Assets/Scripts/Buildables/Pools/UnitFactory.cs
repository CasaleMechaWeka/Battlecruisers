using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Buildables.Pools
{
    public class UnitFactory : IPoolableFactory<Unit, BuildableActivationArgs>
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly IPrefabKey _unitKey;
        private readonly IUIManager _uiManager;
        private readonly IFactoryProvider _factoryProvider;
        private readonly IBuildableWrapper<IUnit> _unitPrefab;

        public UnitFactory(IPrefabFactory prefabFactory, IPrefabKey unitKey, IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            Helper.AssertIsNotNull(prefabFactory, unitKey, uiManager, factoryProvider);

            _prefabFactory = prefabFactory;
            _unitKey = unitKey;
            _uiManager = uiManager;
            _factoryProvider = factoryProvider;

            _unitPrefab = prefabFactory.GetUnitWrapperPrefab(unitKey);
        }

        public Unit CreateItem()
        {
            return
                _prefabFactory
                    .CreateUnit(_unitPrefab, _uiManager, _factoryProvider)
                    .Parse<Unit>();
        }

        public override string ToString()
        {
            return $"{nameof(UnitFactory)} {_unitPrefab.Buildable}";
        }
    }
}
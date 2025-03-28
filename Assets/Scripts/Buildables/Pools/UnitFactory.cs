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
        private readonly IPrefabKey _unitKey;
        private readonly IUIManager _uiManager;
        private readonly FactoryProvider _factoryProvider;
        private readonly IBuildableWrapper<IUnit> _unitPrefab;

        public UnitFactory(IPrefabKey unitKey, IUIManager uiManager, FactoryProvider factoryProvider)
        {
            Helper.AssertIsNotNull(unitKey, uiManager, factoryProvider);

            _unitKey = unitKey;
            _uiManager = uiManager;
            _factoryProvider = factoryProvider;

            _unitPrefab = PrefabFactory.GetUnitWrapperPrefab(unitKey);
        }

        public Unit CreateItem()
        {
            return
                PrefabFactory
                    .CreateUnit(_unitPrefab, _uiManager, _factoryProvider)
                    .Parse<Unit>();
        }

        public override string ToString()
        {
            return $"{nameof(UnitFactory)} {_unitPrefab.Buildable}";
        }
    }
}
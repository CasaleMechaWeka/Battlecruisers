using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Buildables.Pools
{
    public class UnitFactory : IPoolableFactory<Unit, BuildableActivationArgs>
    {
        private readonly IPrefabKey _unitKey;
        private readonly IUIManager _uiManager;
        private readonly IBuildableWrapper<IUnit> _unitPrefab;

        public UnitFactory(IPrefabKey unitKey, IUIManager uiManager)
        {
            Helper.AssertIsNotNull(unitKey, uiManager);

            _unitKey = unitKey;
            _uiManager = uiManager;

            _unitPrefab = PrefabFactory.GetUnitWrapperPrefab(unitKey);
        }

        public Unit CreateItem()
        {
            return
                PrefabFactory
                    .CreateUnit(_unitPrefab, _uiManager)
                    .Parse<Unit>();
        }

        public override string ToString()
        {
            return $"{nameof(UnitFactory)} {_unitPrefab.Buildable}";
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.FactoryManagers
{
    public class FactoryManagerFactory : IFactoryManagerFactory
    {
        private readonly IStaticData _staticData;
        private readonly IPrefabFactory _prefabFactory;

        public FactoryManagerFactory(IStaticData staticData, IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(staticData, prefabFactory);

            _staticData = staticData;
            _prefabFactory = prefabFactory;
        }

        public IFactoryManager CreateNavalFactoryManager(int levelNum, ICruiserController friendlyCruiser)
        {
            IList<IPrefabKey> availableShipKeys = _staticData.GetAvailableUnits(UnitCategory.Naval, levelNum);
            IList<IBuildableWrapper<IUnit>> availableShips =
                availableShipKeys
                    .Select(key => _prefabFactory.GetUnitWrapperPrefab(key))
                    .ToList();
            IUnitChooser unitChooser = new MostExpensiveUnitChooser(availableShips);

            return new FactoryManager(UnitCategory.Naval, friendlyCruiser, unitChooser);
        }
    }
}

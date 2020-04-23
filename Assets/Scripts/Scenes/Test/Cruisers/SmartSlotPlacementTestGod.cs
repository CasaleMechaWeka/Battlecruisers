using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Cruisers
{
    public class SmartSlotPlacementTestGod : TestGodBase
    {
        private IList<CruiserRegionController> _cruiserRegions;

        public BCUtils.PrefabKeyName deckSlotBuilding;

        protected override List<GameObject> GetGameObjects()
        {
            _cruiserRegions = GetComponentsInChildren<CruiserRegionController>();
            Assert.IsTrue(_cruiserRegions.Count > 0);

            foreach (CruiserRegionController cruiserRegion in _cruiserRegions)
            {
                cruiserRegion.StaticInitialise();
            }

            return
                _cruiserRegions
                    .Select(region => region.Cruiser.GameObject)
                    .ToList();
        }

        protected override void Setup(Helper helper)
        {
            foreach (CruiserRegionController cruiserRegion in _cruiserRegions)
            {
                cruiserRegion.Initialise(helper, deckSlotBuilding);
            }
        }
    }
}
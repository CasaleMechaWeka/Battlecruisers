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
        private CameraSwitcher _cameraSwitcher;

        public Camera overviewCamera;
        public BCUtils.PrefabKeyName deckSlotBuilding, platformSlotBuilding, mastSlotBuilding;

        protected override List<GameObject> GetGameObjects()
        {
            Assert.IsNotNull(overviewCamera);

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
            IList<BCUtils.PrefabKeyName> buildingKeys = new List<BCUtils.PrefabKeyName>()
            {
                BCUtils.PrefabKeyName.Building_NavalFactory, // Only building for slot type
                BCUtils.PrefabKeyName.Building_DroneStation, // Only building for slot type
                deckSlotBuilding,
                platformSlotBuilding,
                mastSlotBuilding
            };

            _cameraSwitcher = new CameraSwitcher();
            _cameraSwitcher.ActiveCamera = Camera.main;

            foreach (CruiserRegionController cruiserRegion in _cruiserRegions)
            {
                cruiserRegion.Initialise(_cameraSwitcher, helper, buildingKeys);
            }
        }

        public void ShowOverview()
        {
            BCUtils.Logging.LogMethod(BCUtils.Tags.ALWAYS);
            _cameraSwitcher.ActiveCamera = overviewCamera;
        }
    }
}
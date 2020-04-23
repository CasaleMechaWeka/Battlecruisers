using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Cruisers
{
    public class CruiserRegionController : MonoBehaviour, IPointerClickHandler
    {
        private CameraSwitcher _cameraSwitcher;
        
        public Camera camera;

        public Cruiser cruiser;
        public ICruiser Cruiser => cruiser;

        public void StaticInitialise()
        {
            BCUtils.Helper.AssertIsNotNull(camera, cruiser);
            camera.enabled = false;
        }

        public void Initialise(CameraSwitcher cameraSwitcher, Helper helper, IList<BCUtils.PrefabKeyName> buildingKeyNames)
        {
            BCUtils.Helper.AssertIsNotNull(cameraSwitcher, helper, buildingKeyNames);

            _cameraSwitcher = cameraSwitcher;
            helper.SetupCruiser(cruiser);

            foreach (BCUtils.PrefabKeyName buildingKeyName in buildingKeyNames)
            {
                BuildingKey buildingKey = BCUtils.StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(buildingKeyName);
                IBuildableWrapper<IBuilding> building = helper.PrefabFactory.GetBuildingWrapperPrefab(buildingKey);

                IList<ISlot> freeSlots = cruiser.SlotAccessor.GetFreeSlots(building.Buildable.SlotSpecification.SlotType);
                foreach (ISlot slot in freeSlots)
                {
                    cruiser.ConstructBuilding(building, slot);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            BCUtils.Logging.LogMethod(BCUtils.Tags.ALWAYS);
            _cameraSwitcher.ActiveCamera = camera;
        }
    }
}
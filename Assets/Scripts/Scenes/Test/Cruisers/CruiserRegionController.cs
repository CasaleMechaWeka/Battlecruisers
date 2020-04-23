using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Cruisers
{
    public class CruiserRegionController : MonoBehaviour, IPointerClickHandler
    {
        private CameraSwitcher _cameraSwitcher;
        
        public Camera camera;

        public Cruiser Cruiser { get; private set; }

        public void StaticInitialise()
        {
            Assert.IsNotNull(camera);
            // FELIX
            camera.gameObject.SetActive(false);
            //camera.enabled = false;

            Cruiser cruiser = GetComponentInChildren<Cruiser>();
            Assert.IsNotNull(cruiser);
            Cruiser = cruiser;
        }

        public void Initialise(CameraSwitcher cameraSwitcher, Helper helper, IList<BCUtils.PrefabKeyName> buildingKeyNames)
        {
            BCUtils.Helper.AssertIsNotNull(cameraSwitcher, helper, buildingKeyNames);

            _cameraSwitcher = cameraSwitcher;
            helper.SetupCruiser(Cruiser);

            foreach (BCUtils.PrefabKeyName buildingKeyName in buildingKeyNames)
            {
                BuildingKey buildingKey = BCUtils.StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(buildingKeyName);
                IBuildableWrapper<IBuilding> building = helper.PrefabFactory.GetBuildingWrapperPrefab(buildingKey);

                IList<ISlot> freeSlots = Cruiser.SlotAccessor.GetFreeSlots(building.Buildable.SlotSpecification.SlotType);
                foreach (ISlot slot in freeSlots)
                {
                    Cruiser.ConstructBuilding(building, slot);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            BCUtils.Logging.LogMethod(BCUtils.Tags.ALWAYS, $"{Cruiser}");
            _cameraSwitcher.ActiveCamera = camera;
        }
    }
}
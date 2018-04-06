using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class HUDCanvasController : MonoBehaviour, IHUDCanvasController
    {
        private BuildingDetailsController _buildingDetails;
        public IBuildableDetails<IBuilding> BuildingDetails { get { return _buildingDetails; } }

        private UnitDetailsController _unitDetails;
        public IBuildableDetails<IUnit> UnitDetails { get { return _unitDetails; } }

        private CruiserDetailsController _cruiserDetails;
        public ICruiserDetails CruiserDetails { get { return _cruiserDetails; } }

        public CruiserInfoController PlayerCruiserInfo { get; private set; }
        public CruiserInfoController AICruiserInfo { get; private set;  }

        public void StaticInitialise()
        {
            _buildingDetails = GetComponentInChildren<BuildingDetailsController>(includeInactive: true);
            Assert.IsNotNull(_buildingDetails);

            _unitDetails = GetComponentInChildren<UnitDetailsController>(includeInactive: true);
            Assert.IsNotNull(_unitDetails);

            _cruiserDetails = GetComponentInChildren<CruiserDetailsController>(includeInactive: true);
            Assert.IsNotNull(_cruiserDetails);

            PlayerCruiserInfo = transform.FindNamedComponent<CruiserInfoController>("PlayerCruiserInfo");
            AICruiserInfo = transform.FindNamedComponent<CruiserInfoController>("AICruiserInfo");
        }

        public void Initialise(ISpriteProvider spriteProvider, IDroneManager droneManager, IRepairManager repairManager)
        {
            _buildingDetails.Initialise(spriteProvider, droneManager, repairManager);
            _unitDetails.Initialise(droneManager, repairManager);
            _cruiserDetails.Initialise(droneManager, repairManager);
        }
    }
}

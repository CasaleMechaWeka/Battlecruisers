using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class BuildMenuCanvasController : MonoBehaviour, IBuildMenuCanvasController
    {
		public HealthBarController PlayerCruiserHealthBar { get; private set; }
		public HealthBarController AiCruiserHealthBar { get; private set; }

        private BuildingDetailsController _buildingDetails;
        public IBuildableDetails<IBuilding> BuildingDetails { get { return _buildingDetails; } }

        private UnitDetailsController _unitDetails;
        public IBuildableDetails<IUnit> UnitDetails { get { return _unitDetails; } }

        private CruiserDetailsController _cruiserDetails;
        public ICruiserDetails CruiserDetails { get { return _cruiserDetails; } }

        public void StaticInitialise()
        {
            _buildingDetails = GetComponentInChildren<BuildingDetailsController>(includeInactive: true);
            Assert.IsNotNull(_buildingDetails);

            _unitDetails = GetComponentInChildren<UnitDetailsController>(includeInactive: true);
            Assert.IsNotNull(_unitDetails);

            _cruiserDetails = GetComponentInChildren<CruiserDetailsController>(includeInactive: true);
            Assert.IsNotNull(_cruiserDetails);

            PlayerCruiserHealthBar = transform.FindNamedComponent<HealthBarController>("PlayerCruiserHealthBar");
            AiCruiserHealthBar = transform.FindNamedComponent<HealthBarController>("AiCruiserHealthBar");
        }

        public void Initialise(ISpriteProvider spriteProvider, IDroneManager droneManager, IRepairManager repairManager)
        {
            _buildingDetails.Initialise(spriteProvider, droneManager, repairManager);
            _unitDetails.Initialise(droneManager, repairManager);
            _cruiserDetails.Initialise(droneManager, repairManager);
        }
    }
}

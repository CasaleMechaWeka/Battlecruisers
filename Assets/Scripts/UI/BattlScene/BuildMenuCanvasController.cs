using BattleCruisers.Buildables.Repairables;
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
        public BuildingDetailsController BuildingDetails { get; private set; }
        public UnitDetailsController UnitDetails { get; private set; }
        public InBattleCruiserDetailsController CruiserDetails { get; private set; }
        public HealthBarController PlayerCruiserHealthBar { get; private set; }
        public HealthBarController AiCruiserHealthBar { get; private set; }

        public void StaticInitialise()
        {
            BuildingDetails = GetComponentInChildren<BuildingDetailsController>(includeInactive: true);
            Assert.IsNotNull(BuildingDetails);

            UnitDetails = GetComponentInChildren<UnitDetailsController>(includeInactive: true);
            Assert.IsNotNull(UnitDetails);

            CruiserDetails = GetComponentInChildren<InBattleCruiserDetailsController>(includeInactive: true);
            Assert.IsNotNull(CruiserDetails);

            PlayerCruiserHealthBar = transform.FindNamedComponent<HealthBarController>("PlayerCruiserHealthBar");
            AiCruiserHealthBar = transform.FindNamedComponent<HealthBarController>("AiCruiserHealthBar");
        }

        public void Initialise(ISpriteProvider spriteProvider, IDroneManager droneManager, IRepairManager repairManager)
        {
            BuildingDetails.Initialise(spriteProvider, droneManager, repairManager);
            UnitDetails.Initialise(droneManager, repairManager);
            CruiserDetails.Initialise(droneManager, repairManager);
        }
    }
}

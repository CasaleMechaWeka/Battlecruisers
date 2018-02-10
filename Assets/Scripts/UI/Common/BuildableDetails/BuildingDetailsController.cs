using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.Common.BuildingDetails.Buttons;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class BuildingDetailsController : BuildableDetailsController<IBuilding>
    {
        private SlotTypeController _slotType;

        public void Initialise(ISpriteProvider spriteProvider, IDroneManager droneManager, IRepairManager repairManager)
        {
            base.Initialise(droneManager, repairManager);

            _slotType = GetComponentInChildren<SlotTypeController>();
            Assert.IsNotNull(_slotType);
            _slotType.Initialise(spriteProvider);
        }

        protected override StatsController<IBuilding> GetStatsController()
        {
            return GetComponentInChildren<BuildingStatsController>();
        }

        public override void ShowBuildableDetails(IBuilding buildable, bool allowDelete)
        {
            base.ShowBuildableDetails(buildable, allowDelete);
            _slotType.SlotType = buildable.SlotType;
        }
    }
}

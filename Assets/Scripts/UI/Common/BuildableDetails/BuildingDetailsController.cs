using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class BuildingDetailsController : BuildableDetailsController<IBuilding>
    {
        private SlotTypeController _slotType;

        public void Initialise(
            ISpriteProvider spriteProvider, 
            IDroneManager droneManager, 
            IRepairManager repairManager, 
            IUserChosenTargetHelper userChosenTargetHelper,
            IFilter<ITarget> chooseTargetButtonVisibilityFilter,
            IFilter<ITarget> deleteButtonVisibilityFilter)
        {
            base.Initialise(droneManager, repairManager, userChosenTargetHelper, chooseTargetButtonVisibilityFilter, deleteButtonVisibilityFilter);

            Assert.IsNotNull(spriteProvider);

            _slotType = GetComponentInChildren<SlotTypeController>();
            Assert.IsNotNull(_slotType);
            _slotType.Initialise(spriteProvider);
        }

        protected override StatsController<IBuilding> GetStatsController()
        {
            return GetComponentInChildren<BuildingStatsController>();
        }

        public override void ShowBuildableDetails(IBuilding buildable)
        {
            base.ShowBuildableDetails(buildable);
            _slotType.SlotType = buildable.SlotType;
        }
    }
}

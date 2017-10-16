using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI.Common.BuildingDetails.Stats;

// FELIX  Avoid duplicate toggle repair code with buildable details controller
namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class InBattleCruiserDetailsController : CruiserDetailsController
    {
        public void Initialise(IDroneManager droneManager, IRepairManager repairManager)
        {
            base.Initialise();

            // FELIX
        }

        public void ShowCruiserDetails(Cruiser cruiser)
        {
            base.ShowItemDetails(cruiser);

            // FELIX
        }
    }
}

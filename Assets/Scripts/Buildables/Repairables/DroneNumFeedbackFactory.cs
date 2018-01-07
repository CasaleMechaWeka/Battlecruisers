using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils.UIWrappers;

namespace BattleCruisers.Buildables.Repairables
{
    public class DroneNumFeedbackFactory : IDroneNumFeedbackFactory
    {
        public IDroneNumFeedback CreateFeedback(IDroneConsumer repairDroneConsumer, ITextMesh numOfDronesText)
        {
            return new DroneNumFeedback(repairDroneConsumer, numOfDronesText);
        }
    }
}

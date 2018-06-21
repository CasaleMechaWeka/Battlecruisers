using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.Buildables.Repairables
{
    public class DummyDroneNumFeedbackFactory : IDroneNumFeedbackFactory
    {
        public IDroneNumFeedback CreateFeedback(IDroneConsumer repairDroneConsumer, ITextMesh numOfDronesText)
        {
            return new DroneNumFeedbackBase(repairDroneConsumer, numOfDronesText);
        }
    }
}

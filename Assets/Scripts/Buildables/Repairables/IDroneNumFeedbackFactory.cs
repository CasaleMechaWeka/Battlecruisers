using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.Buildables.Repairables
{
    // FELIX  Remove :)
    public interface IDroneNumFeedbackFactory
    {
        IDroneNumFeedback CreateFeedback(IDroneConsumer repairDroneConsumer, ITextMesh numOfDronesText);
    }
}

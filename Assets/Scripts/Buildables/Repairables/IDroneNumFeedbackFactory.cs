using BattleCruisers.Drones;
using BattleCruisers.Utils.UIWrappers;

namespace BattleCruisers.Buildables.Repairables
{
    public interface IDroneNumFeedbackFactory
    {
        IDroneNumFeedback CreateFeedback(IDroneConsumer repairDroneConsumer, ITextMesh numOfDronesText);
    }
}

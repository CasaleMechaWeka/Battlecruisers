using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.Buildables.Repairables
{
    /// <summary>
    /// Shows no feedaback for the number of repair drones.  Useful for the AI
    /// cruiser, where we do not want to display any feedback.
    /// </summary>
    public class DroneNumFeedbackBase : IDroneNumFeedback
    {
        protected readonly ITextMesh _textMesh;

        public IDroneConsumer DroneConsumer { get; }

        public DroneNumFeedbackBase(IDroneConsumer droneConsumer, ITextMesh textMesh)
        {
            Helper.AssertIsNotNull(droneConsumer, textMesh);

            DroneConsumer = droneConsumer;
            _textMesh = textMesh;
        }

        public virtual void DisposeManagedState() { }
    }
}

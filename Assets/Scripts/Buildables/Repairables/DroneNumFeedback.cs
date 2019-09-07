using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.Buildables.Repairables
{
    /// <summary>
    /// Shows the number of repair drones via the text mesh.
    /// </summary>
    /// FELIX  Can reuse this for showing drone animations?
    public class DroneNumFeedback : DroneNumFeedbackBase
    {
        private const string FEEDBACK_PREFIX = "R";  // R for "Repair"

        public DroneNumFeedback(IDroneConsumer droneConsumer, ITextMesh textMesh)
            : base(droneConsumer, textMesh)
        {
            DroneConsumer.DroneNumChanged += DroneConsumer_DroneNumChanged;
        }

        private void DroneConsumer_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            _textMesh.Text = FEEDBACK_PREFIX + e.NewNumOfDrones;
            _textMesh.SetActive(e.NewNumOfDrones != 0);
        }

        public override void DisposeManagedState()
        {
            DroneConsumer.DroneNumChanged -= DroneConsumer_DroneNumChanged;
        }
    }
}

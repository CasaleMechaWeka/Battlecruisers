using BattleCruisers.Drones;
using BattleCruisers.Utils;
using BattleCruisers.Utils.UIWrappers;

namespace BattleCruisers.Buildables.Repairables
{
    public class DroneNumFeedback : IDroneNumFeedback
    {
        private readonly ITextMesh _textMesh;

        public IDroneConsumer DroneConsumer { get; private set; }

        public DroneNumFeedback(IDroneConsumer droneConsumer, ITextMesh textMesh)
        {
            Helper.AssertIsNotNull(droneConsumer, textMesh);

            DroneConsumer = droneConsumer;
            _textMesh = textMesh;

            DroneConsumer.DroneNumChanged += DroneConsumer_DroneNumChanged;
        }

        private void DroneConsumer_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            _textMesh.Text = e.NewNumOfDrones.ToString();
            _textMesh.SetActive(e.NewNumOfDrones != 0);
        }

        public void DisposeManagedState()
        {
            DroneConsumer.DroneNumChanged -= DroneConsumer_DroneNumChanged;
        }
    }
}

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    // null object
    public class DummyDroneFeedback : IDroneFeedback
    {
        public IDroneConsumer DroneConsumer => null;

        public void DisposeManagedState()
        {
            // empty
        }
    }
}
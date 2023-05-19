namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    // null object
    public class PvPDummyDroneFeedback : IPvPDroneFeedback
    {
        public IPvPDroneConsumer DroneConsumer => null;

        public void DisposeManagedState()
        {
            // empty
        }
    }
}
using BattleCruisers.Cruisers.Drones;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    // null object
    public class PvPDummyDroneFeedback : IPvPDroneFeedback
    {
        public IDroneConsumer DroneConsumer => null;

        public void DisposeManagedState()
        {
            // empty
        }
    }
}
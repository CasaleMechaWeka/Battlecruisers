using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Drones.Feedback;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    // null object
    public class PvPDummyDroneFeedback : IDroneFeedback
    {
        public IDroneConsumer DroneConsumer => null;

        public void DisposeManagedState()
        {
            // empty
        }
    }
}
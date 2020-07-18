using System;

namespace BattleCruisers.Cruisers.Drones
{
    public interface IDroneFocuser
    {
        event EventHandler PlayerTriggeredRepair;

        void ToggleDroneConsumerFocus(IDroneConsumer droneConsumer, bool isTriggeredByPlayer);
    }
}
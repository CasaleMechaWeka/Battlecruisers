namespace BattleCruisers.Cruisers.Drones
{
    public interface IDroneFocuser
    {
        void ToggleDroneConsumerFocus(IDroneConsumer droneConsumer, bool isTriggeredByPlayer);
    }
}
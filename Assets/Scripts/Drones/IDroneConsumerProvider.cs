namespace BattleCruisers.Drones
{
    public interface IDroneConsumerProvider
    {
        IDroneConsumer RequestDroneConsumer(int numOfDronesRequired, bool isHighPriority);
        void ActivateDroneConsumer(IDroneConsumer droneConsumer);
        void ReleaseDroneConsumer(IDroneConsumer droneConsumer);
    }
}

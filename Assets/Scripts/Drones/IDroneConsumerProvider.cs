namespace BattleCruisers.Drones
{
    public interface IDroneConsumerProvider
    {
        IDroneConsumer RequestDroneConsumer(int numOfDronesRequired);
        void ActivateDroneConsumer(IDroneConsumer droneConsumer);
        void ReleaseDroneConsumer(IDroneConsumer droneConsumer);
    }
}

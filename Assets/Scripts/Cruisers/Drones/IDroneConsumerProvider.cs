namespace BattleCruisers.Cruisers.Drones
{
    public interface IDroneConsumerProvider
    {
        IDroneConsumer RequestDroneConsumer(int numOfDronesRequired);

        /// <summary>
        /// Activating a drone consumer that cannot be supported (ie, not enough
        /// drones available) or that has already been activated (without being 
        /// released) throws.
        /// </summary>
        void ActivateDroneConsumer(IDroneConsumer droneConsumer);

        /// <summary>
        /// Releasing a drone consumer that has not previously been activated
        /// is ok.  If this is the case this method does nothing.
        /// </summary>
        void ReleaseDroneConsumer(IDroneConsumer droneConsumer);
    }
}

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public interface IPvPDroneConsumerProvider
    {
        IPvPDroneConsumer RequestDroneConsumer(int numOfDronesRequired);

        /// <summary>
        /// Activating a drone consumer that cannot be supported (ie, not enough
        /// drones available) or that has already been activated (without being 
        /// released) throws.
        /// </summary>
        void ActivateDroneConsumer(IPvPDroneConsumer droneConsumer);

        /// <summary>
        /// Releasing a drone consumer that has not previously been activated
        /// is ok.  If this is the case this method does nothing.
        /// </summary>
        void ReleaseDroneConsumer(IPvPDroneConsumer droneConsumer);
    }
}

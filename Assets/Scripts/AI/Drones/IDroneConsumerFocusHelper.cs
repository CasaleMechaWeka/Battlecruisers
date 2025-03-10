namespace BattleCruisers.AI.Drones
{
    public interface IDroneConsumerFocusHelper
    {
        void FocusOnNonFactoryDroneConsumer(bool forceInProgressBuildingToFocused);
    }
}

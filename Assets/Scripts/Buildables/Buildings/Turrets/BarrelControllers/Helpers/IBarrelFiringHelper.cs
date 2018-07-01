namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public interface IBarrelFiringHelper
    {
        /// <returns><c>true</c>, if successfully fired, <c>false</c> otherwise.</returns>
        bool TryFire(BarrelAdjustmentResult barrelAdjustmentResult);
    }
}

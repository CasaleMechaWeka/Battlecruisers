namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public interface IPvPBarrelFiringHelper
    {
        /// <returns><c>true</c>, if successfully fired, <c>false</c> otherwise.</returns>
        bool TryFire(PvPBarrelAdjustmentResult barrelAdjustmentResult);
    }
}

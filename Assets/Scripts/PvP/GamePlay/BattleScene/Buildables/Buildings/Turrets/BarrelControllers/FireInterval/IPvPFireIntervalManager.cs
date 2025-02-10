using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public interface IPvPFireIntervalManager
    {
        IBroadcastingProperty<bool> ShouldFire { get; }

        void OnFired();
        void ProcessTimeInterval(float deltaTime);
    }
}

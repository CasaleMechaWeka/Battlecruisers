using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public interface IPvPFireIntervalManager
    {
        IPvPBroadcastingProperty<bool> ShouldFire { get; }

        void OnFired();
        void ProcessTimeInterval(float deltaTime);
    }
}

using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Laser
{
    public interface IPvPLaserEmitter : IPvPBeamEmitter
    {
        IPvPBroadcastingProperty<bool> IsLaserFiring { get; }

        void StopLaser();
    }
}
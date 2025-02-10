using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams
{
    public interface IPvPBeamEmitter : IManagedDisposable
    {
        void FireBeam(float angleInDegrees, bool isSourceMirrored);
    }
}
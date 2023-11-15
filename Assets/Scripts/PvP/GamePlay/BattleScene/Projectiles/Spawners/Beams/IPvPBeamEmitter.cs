using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams
{
    public interface IPvPBeamEmitter : IPvPManagedDisposable
    {
        void FireBeam(float angleInDegrees, bool isSourceMirrored);
    }
}
using BattleCruisers.Utils;

namespace BattleCruisers.Projectiles.Spawners.Beams
{
    public interface IBeamEmitter : IManagedDisposable
    {
        void FireBeam(float angleInDegrees, bool isSourceMirrored);
    }
}
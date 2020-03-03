using BattleCruisers.Projectiles.Spawners.Laser;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Effects.Laser
{
    public class DummyLaserCooldownEffectInitialiser : MonoBehaviour, ILaserCooldownEffectInitialiser
    {
        public IManagedDisposable CreateLaserCooldownEffect(ILaserEmitter laserEmitter)
        {
            return new DummyManagedDisposable();
        }
    }
}

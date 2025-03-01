using BattleCruisers.Effects.Laser;
using BattleCruisers.Projectiles.Spawners.Beams.Laser;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser
{
    public class PvPDummyLaserCooldownEffectInitialiser : MonoBehaviour, ILaserCooldownEffectInitialiser
    {
        public IManagedDisposable CreateLaserCooldownEffect(ILaserEmitter laserEmitter)
        {
            return new DummyManagedDisposable();
        }
    }
}

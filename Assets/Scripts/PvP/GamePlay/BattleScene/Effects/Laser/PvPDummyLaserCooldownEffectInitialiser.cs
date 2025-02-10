using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Projectiles.Spawners.Beams.Laser;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser
{
    public class PvPDummyLaserCooldownEffectInitialiser : MonoBehaviour, IPvPLaserCooldownEffectInitialiser
    {
        public IManagedDisposable CreateLaserCooldownEffect(ILaserEmitter laserEmitter)
        {
            return new PvPDummyManagedDisposable();
        }
    }
}

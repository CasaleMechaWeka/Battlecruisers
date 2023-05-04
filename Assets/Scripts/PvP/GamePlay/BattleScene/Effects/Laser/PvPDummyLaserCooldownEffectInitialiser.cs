using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Laser;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser
{
    public class PvPDummyLaserCooldownEffectInitialiser : MonoBehaviour, IPvPLaserCooldownEffectInitialiser
    {
        public IPvPManagedDisposable CreateLaserCooldownEffect(IPvPLaserEmitter laserEmitter)
        {
            return new PvPDummyManagedDisposable();
        }
    }
}

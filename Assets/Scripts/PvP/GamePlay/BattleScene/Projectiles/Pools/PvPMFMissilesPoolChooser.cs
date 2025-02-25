using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMFMissilesPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        public IPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesMFPool;
        }
    }
}
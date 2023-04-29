using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMissilesSmallPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        public IPvPPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesSmallPool;
        }
    }
}
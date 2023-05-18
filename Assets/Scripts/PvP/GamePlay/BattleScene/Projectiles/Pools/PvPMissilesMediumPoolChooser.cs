using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMissilesMediumPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        public IPvPPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesMediumPool;
        }
    }
}
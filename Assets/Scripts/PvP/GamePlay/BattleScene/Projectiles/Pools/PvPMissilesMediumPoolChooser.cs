using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMissilesMediumPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>, IProjectileStats>
    {
        public IPool<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesMediumPool;
        }
    }
}
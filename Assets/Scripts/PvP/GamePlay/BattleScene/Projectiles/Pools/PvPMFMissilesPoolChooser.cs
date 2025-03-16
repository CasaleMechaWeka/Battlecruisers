using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMFMissilesPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPMissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>
    {
        public IPool<PvPMissileController, TargetProviderActivationArgs<IProjectileStats>> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesMFPool;
        }
    }
}
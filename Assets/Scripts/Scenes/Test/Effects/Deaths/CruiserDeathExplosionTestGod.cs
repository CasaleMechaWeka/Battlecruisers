using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils.BattleScene.Pools;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class CruiserDeathExplosionTestGod : MonoBehaviour
    {
        public List<ExplosionController> cruiserDeathExplosions;

        void Start()
        {
            foreach (ExplosionController cruiserDeath in cruiserDeathExplosions)
            {
                if (!cruiserDeath.isActiveAndEnabled)
                {
                    continue;
                }

                IPoolable<Vector3> explosion = cruiserDeath.Initialise();
                explosion.Activate(cruiserDeath.Position);
            }
        }
    }
}
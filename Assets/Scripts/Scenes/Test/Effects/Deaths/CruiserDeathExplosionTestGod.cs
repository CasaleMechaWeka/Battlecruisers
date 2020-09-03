using BattleCruisers.Effects.Explosions;
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
                IExplosion explosion = cruiserDeath.Initialise();
                explosion.Activate(cruiserDeath.Position);
            }
        }
    }
}
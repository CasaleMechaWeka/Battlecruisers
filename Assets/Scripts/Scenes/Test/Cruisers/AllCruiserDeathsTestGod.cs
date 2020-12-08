using BattleCruisers.Effects.Explosions;
using UnityEngine;

namespace Assets.Scripts.Scenes.Test.Cruisers
{
    public class AllCruiserDeathsTestGod : MonoBehaviour
    {
        void Start()
        {
            ExplosionController[] cruiserDeaths = FindObjectsOfType<ExplosionController>();

            foreach (ExplosionController death in cruiserDeaths)
            {
                IExplosion deathExplosion = death.Initialise();
                deathExplosion.Activate(death.Position);
            }
        }
    }
}
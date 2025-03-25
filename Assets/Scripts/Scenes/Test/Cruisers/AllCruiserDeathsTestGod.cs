using BattleCruisers.Data;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Cruisers
{
    public class AllCruiserDeathsTestGod : MonoBehaviour
    {
        void Start()
        {
            CruiserDeathExplosion[] cruiserDeaths = FindObjectsOfType<CruiserDeathExplosion>();

            foreach (CruiserDeathExplosion death in cruiserDeaths)
            {
                IPoolable<Vector3> deathExplosion = death.Initialise(DataProvider.SettingsManager);
                deathExplosion.Activate(death.Position);
            }
        }
    }
}
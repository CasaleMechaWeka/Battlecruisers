using BattleCruisers.Data;
using BattleCruisers.Effects.Explosions;
using UnityEngine;

// FELIX  fix :D
namespace Assets.Scripts.Scenes.Test.Cruisers
{
    public class AllCruiserDeathsTestGod : MonoBehaviour
    {
        void Start()
        {
            CruiserDeathExplosion[] cruiserDeaths = FindObjectsOfType<CruiserDeathExplosion>();

            foreach (CruiserDeathExplosion death in cruiserDeaths)
            {
                IExplosion deathExplosion = death.Initialise(ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager);
                deathExplosion.Activate(death.Position);
            }
        }
    }
}
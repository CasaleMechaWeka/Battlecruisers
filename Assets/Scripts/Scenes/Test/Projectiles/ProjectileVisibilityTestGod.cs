using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Projectiles
{
    public class ProjectileVisibilityTestGod : MonoBehaviour
    {
        // FELIX  Allow changing of skybox :)

        void Start()
        {
            Helper helper = new Helper();

            Artillery artillery = FindObjectOfType<Artillery>();
            helper.InitialiseBuilding(artillery, Faction.Blues);
            artillery.StartConstruction();

            AirFactory airFactory = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(airFactory, Faction.Reds);
            airFactory.StartConstruction();
        }

        void Update()
        {

        }
    }
}
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Projectiles
{
    public class ProjectileVisibilityTestGod : NavigationTestGod
    {
        private Artillery _artillery;
        private AirFactory _airFactory;

        protected override List<GameObject> GetGameObjects()
        {
            _artillery = FindObjectOfType<Artillery>();
            _airFactory = FindObjectOfType<AirFactory>();

            return new List<GameObject>()
            {
                _artillery.GameObject,
                _airFactory.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);

            helper.InitialiseBuilding(_artillery, Faction.Blues);
            _artillery.StartConstruction();

            helper.InitialiseBuilding(_airFactory, Faction.Reds);
            _airFactory.StartConstruction();
        }
    }
}
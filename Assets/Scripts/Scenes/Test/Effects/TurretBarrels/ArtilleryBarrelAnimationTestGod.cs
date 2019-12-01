using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.TurretBarrels
{
    public class ArtilleryBarrelAnimationTestGod : TestGodBase
    {
        public TurretController turret;

        protected override List<GameObject> GetGameObjects()
        {
            Assert.IsNotNull(turret);
            return new List<GameObject>()
            { 
                turret.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);
            Debug.Log("Setup()");

            helper.InitialiseBuilding(turret);
            turret.StartConstruction();
        }
    }
}
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Effects;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.TurretBarrels
{
    public class ArtilleryBarrelAnimationTestGod : TestGodBase
    {
        private IAnimation _barrelAnimation;

        public TurretController turret;

        protected override List<GameObject> GetGameObjects()
        {
            Assert.IsNotNull(turret);
            return new List<GameObject>()
            { 
                turret.GameObject
            };
        }

        // FELIX  NEXT
        // 1. Remove redundant sprites
        // 3. Use BarrelAnimationInitialiser in inspector :)
        protected override void Setup(Helper helper)
        {
            base.Setup(helper);
            Debug.Log("Setup()");

            helper.InitialiseBuilding(turret);
            turret.StartConstruction();
        }

        public void PlayAnimation()
        {
            Debug.Log("PlayAnimation()");
            _barrelAnimation.Play();
        }
    }
}
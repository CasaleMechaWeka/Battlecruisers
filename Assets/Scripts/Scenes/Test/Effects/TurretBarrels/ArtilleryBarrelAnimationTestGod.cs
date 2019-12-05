using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects.TurretBarrels
{
    // FELIX  Rename to be generic to all turrets :)
    public class ArtilleryBarrelAnimationTestGod : TestGodBase
    {
        private TurretController[] _turrets;

        protected override List<GameObject> GetGameObjects()
        {
            _turrets = FindObjectsOfType<TurretController>();
            return
                _turrets
                    .Select(turret => turret.GameObject)
                    .ToList();
        }

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);
            Debug.Log("Setup()");

            foreach (TurretController turret in _turrets)
            {
                helper.InitialiseBuilding(turret);
                turret.StartConstruction();
            }
        }
    }
}
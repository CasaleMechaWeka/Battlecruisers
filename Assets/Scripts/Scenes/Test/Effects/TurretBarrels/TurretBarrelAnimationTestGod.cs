using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects.TurretBarrels
{
    public class TurretBarrelAnimationTestGod : TestGodBase
    {
        private TurretController[] _turrets;

        protected override async Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            // May be null
            IDeferrer deferrer = GetComponent<IDeferrer>();

            return await HelperFactory.CreateHelperAsync(updaterProvider: updaterProvider, deferrer: deferrer);
        }

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
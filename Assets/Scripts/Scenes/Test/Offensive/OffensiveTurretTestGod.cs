using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Offensive
{
    public class OffensiveTurretTestGod : CameraToggleTestGod
	{
        private IBuilding _target;
        private TurretController _turret;

        protected override List<GameObject> GetGameObjects()
        {
            _target = GetTarget();
            _turret = FindObjectOfType<TurretController>();

            return new List<GameObject>()
            {
                _target.GameObject,
                _turret.GameObject
            };
        }

        protected virtual IBuilding GetTarget()
        {
            return FindObjectOfType<AirFactory>();
        }

        protected override async Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            TimeScaleDeferrer deferrer = GetComponent<TimeScaleDeferrer>();
            // Is ok for deferrer to be null :)

            return await HelperFactory.CreateHelperAsync(updaterProvider: updaterProvider, deferrer: deferrer);
        }

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);

            // Setup target
            helper.InitialiseBuilding(_target, Faction.Blues);
			_target.StartConstruction();

			// Setup turret
			ITargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = _target
			};
            ITargetFactories targetFactories = helper.CreateTargetFactories(_target.GameObject, targetFilter: targetFilter);
            helper.InitialiseBuilding(_turret, Faction.Reds, targetFactories: targetFactories);
			_turret.StartConstruction();
		}
	}
}

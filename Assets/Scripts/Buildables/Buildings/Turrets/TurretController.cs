using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class TurretController : Building
	{
		protected IBarrelWrapper _barrelWrapper;

        // By default have null (no) sound
        protected virtual ISoundKey FiringSound { get { return null; } }

        protected override void OnStaticInitialised()
		{
            base.OnStaticInitialised();

            _barrelWrapper = gameObject.GetComponentInChildren<IBarrelWrapper>();
			Assert.IsNotNull(_barrelWrapper);
			_barrelWrapper.StaticInitialise();
            AddDamageStats(_barrelWrapper.DamageCapability);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            _barrelWrapper.Initialise(this, _factoryProvider, enemyFaction, FiringSound);

            _boostableGroup.AddBoostable(_barrelWrapper);
		}

		protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            _barrelWrapper.StartAttackingTargets();
        }

        protected override IList<Renderer> GetInGameRenderers()
        {
            IList<Renderer> renderers = _barrelWrapper.Renderers.ToList();
            renderers.Add(GetBaseRenderer());
            return renderers;
        }

        protected virtual Renderer GetBaseRenderer()
        {
			GameObject turretBase = transform.Find("Base").gameObject;
            Renderer turretBaseRenderer = turretBase.GetComponent<Renderer>();
			Assert.IsNotNull(turretBaseRenderer);
            return turretBaseRenderer;
        }
    }
}

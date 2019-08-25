using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public abstract class TurretController : Building
	{
		protected IBarrelWrapper _barrelWrapper;

        protected override ISoundKey DeathSoundKey => SoundKeys.Deaths.Building2;

        // By default have null (no) sound
        protected virtual ISoundKey FiringSound => null;

        public override void StaticInitialise(HealthBarController healthBar)
		{
            base.StaticInitialise(healthBar);

            _barrelWrapper = gameObject.GetComponentInChildren<IBarrelWrapper>();
			Assert.IsNotNull(_barrelWrapper);
			_barrelWrapper.StaticInitialise();
            AddDamageStats(_barrelWrapper.DamageCapability);
		}

		protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            _barrelWrapper.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction, FiringSound, _parentSlot.BoostProviders, TurretFireRateBoostProviders);
        }

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            List<SpriteRenderer> renderers = _barrelWrapper.Renderers.ToList();
            renderers.Add(GetBaseRenderer());
            return renderers;
        }

        protected virtual SpriteRenderer GetBaseRenderer()
        {
			GameObject turretBase = transform.Find("Base").gameObject;
            SpriteRenderer turretBaseRenderer = turretBase.GetComponent<SpriteRenderer>();
			Assert.IsNotNull(turretBaseRenderer);
            return turretBaseRenderer;
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            _barrelWrapper.DisposeManagedState();
        }
    }
}

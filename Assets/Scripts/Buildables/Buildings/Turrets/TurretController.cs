using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public abstract class TurretController : Building
	{
		protected IBarrelWrapper _barrelWrapper;

        protected override ISoundKey DeathSoundKey { get { return SoundKeys.Deaths.Building2; } }

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

		protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            _barrelWrapper.Initialise(this, _factoryProvider, enemyFaction, FiringSound, _parentSlot.BoostProviders, TurretFireRateBoostProviders);
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
    }
}

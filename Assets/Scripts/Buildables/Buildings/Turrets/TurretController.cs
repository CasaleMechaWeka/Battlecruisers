using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Effects;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public abstract class TurretController : Building
	{
        private IAnimation _barrelAnimation;
		protected IBarrelWrapper _barrelWrapper;

        public override bool IsBoostable => true;

        // By default have null (no) sound
        protected virtual ISoundKey FiringSound => null;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings)
		{
            base.StaticInitialise(parent, healthBar, commonStrings);

            _barrelWrapper = gameObject.GetComponentInChildren<IBarrelWrapper>();
			Assert.IsNotNull(_barrelWrapper);
			_barrelWrapper.StaticInitialise();
            AddDamageStats(_barrelWrapper.DamageCapability);

            IAnimationInitialiser barrelAnimationInitialiser = GetComponent<IAnimationInitialiser>();
            Assert.IsNotNull(barrelAnimationInitialiser);
            _barrelAnimation = barrelAnimationInitialiser.CreateAnimation();
		}

		protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            _barrelWrapper
                .Initialise(
                    this, 
                    _factoryProvider, 
                    _cruiserSpecificFactories, 
                    FiringSound, 
                    _parentSlot.BoostProviders, 
                    TurretFireRateBoostProviders,
                    _barrelAnimation);
        }

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            List<SpriteRenderer> renderers = _barrelWrapper.Renderers.ToList();
            renderers.AddRange(GetBaseRenderers());
            return renderers;
        }

        protected virtual SpriteRenderer[] GetBaseRenderers()
        {
			GameObject turretBase = transform.Find("Base").gameObject;
            SpriteRenderer[] turretBaseRenderers = turretBase.GetComponentsInChildren<SpriteRenderer>();
			Assert.IsTrue(turretBaseRenderers.Length > 0);
            return turretBaseRenderers;
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            _barrelWrapper.DisposeManagedState();
        }
    }
}

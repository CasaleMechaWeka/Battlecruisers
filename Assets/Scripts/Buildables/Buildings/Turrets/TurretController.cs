using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Effects;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public abstract class TurretController : Building
    {
        private IAnimation _barrelAnimation;
        protected IBarrelWrapper _barrelWrapper;

        // By default have null (no) sound
        protected virtual ISoundKey FiringSound => null;
        protected virtual bool HasSingleSprite => false;

        public override bool IsBoostable => true;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

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
                    _cruiserSpecificFactories,
                    FiringSound,
                    _parentSlot.BoostProviders,
                    TurretFireRateBoostProviders,
                    _barrelAnimation);
            _barrelWrapper.ApplyVariantStats(this);
        }

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            List<SpriteRenderer> renderers = _barrelWrapper.Renderers.ToList();
            renderers.AddRange(GetBaseRenderers());
            return renderers;
        }

        protected virtual ICollection<SpriteRenderer> GetBaseRenderers()
        {
            if (HasSingleSprite)
            {
                return base.GetInGameRenderers();
            }
            else
            {
                Transform baseTransform = transform.Find("Base");
                if (baseTransform == null)
                {
                    Debug.LogWarning($"No Base found for turret {gameObject.name}. Returning empty renderer list.");
                    return new SpriteRenderer[0];
                }
                
                GameObject turretBase = baseTransform.gameObject;
                SpriteRenderer[] turretBaseRenderers = turretBase.GetComponentsInChildren<SpriteRenderer>();
                if (turretBaseRenderers.Length == 0)
                {
                    Debug.LogWarning($"No sprite renderers found in Base for {gameObject.name}");
                }
                
                return turretBaseRenderers;
            }
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            _barrelWrapper.DisposeManagedState();
        }
    }
}

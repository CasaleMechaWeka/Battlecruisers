using BattleCruisers.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public class PvPMultiTurretController : PvPBuilding
    {
        private IAnimation _barrelAnimation;
        protected IPvPBarrelWrapper[] _barrelWrappers;

        protected virtual bool HasSingleSprite => false;

        public override bool IsBoostable => true;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            _barrelWrappers = gameObject.GetComponentsInChildren<IPvPBarrelWrapper>();
            Assert.IsNotNull(_barrelWrappers);

            for (int i = 0; i < _barrelWrappers.Count(); i++)
            {
                _barrelWrappers[i].StaticInitialise();
                AddDamageStats(_barrelWrappers[i].DamageCapability);
            }

            IAnimationInitialiser barrelAnimationInitialiser = GetComponent<IAnimationInitialiser>();
            Assert.IsNotNull(barrelAnimationInitialiser);
            _barrelAnimation = barrelAnimationInitialiser.CreateAnimation();
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            for (int i = 0; i < _barrelWrappers.Count(); i++)
            {
                _barrelWrappers[i]
                    .Initialise(
                        this,
                        _cruiserSpecificFactories,
                        _parentSlot.BoostProviders,
                        TurretFireRateBoostProviders,
                        _barrelAnimation);
                _barrelWrappers[i].ApplyVariantStats(this);
            }
        }

        protected override void OnBuildableCompleted_PvPClient()
        {
            base.OnBuildableCompleted_PvPClient();
            for (int i = 0; i < _barrelWrappers.Count(); i++)
            {
                _barrelWrappers[i]
                .Initialise(
                    this,
                    _barrelAnimation);
                _barrelWrappers[i].ApplyVariantStats(this);
            }
        }

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            List<SpriteRenderer> renderers = _barrelWrappers.SelectMany(barrelWrapper => barrelWrapper.Renderers).ToList();
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
                GameObject turretBase = transform.Find("Base").gameObject;
                SpriteRenderer[] turretBaseRenderers = turretBase.GetComponentsInChildren<SpriteRenderer>();
                Assert.IsTrue(turretBaseRenderers.Length > 0);
                return turretBaseRenderers;
            }
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            for (int i = 0; i < _barrelWrappers.Count(); i++)
                _barrelWrappers[i].DisposeManagedState();
        }
    }
}

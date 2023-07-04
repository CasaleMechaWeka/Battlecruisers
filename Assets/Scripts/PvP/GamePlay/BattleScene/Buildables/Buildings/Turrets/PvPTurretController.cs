using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public abstract class PvPTurretController : PvPBuilding
    {
        private IPvPAnimation _barrelAnimation;
        protected IPvPBarrelWrapper _barrelWrapper;

        // By default have null (no) sound
        protected virtual IPvPSoundKey FiringSound => null;
        protected virtual bool HasSingleSprite => false;

        public override bool IsBoostable => true;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _barrelWrapper = gameObject.GetComponentInChildren<IPvPBarrelWrapper>();
            Assert.IsNotNull(_barrelWrapper);
            _barrelWrapper.StaticInitialise();
            AddDamageStats(_barrelWrapper.DamageCapability);

            IPvPAnimationInitialiser barrelAnimationInitialiser = GetComponent<IPvPAnimationInitialiser>();
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

        protected override void OnBuildableCompleted_PvPClient()
        {
            base.OnBuildableCompleted_PvPClient();
           
            _barrelWrapper
            .Initialise(
                this,
                _factoryProvider,
                FiringSound,
                _barrelAnimation);
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
                GameObject turretBase = transform.Find("Base").gameObject;
                SpriteRenderer[] turretBaseRenderers = turretBase.GetComponentsInChildren<SpriteRenderer>();
                Assert.IsTrue(turretBaseRenderers.Length > 0);
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

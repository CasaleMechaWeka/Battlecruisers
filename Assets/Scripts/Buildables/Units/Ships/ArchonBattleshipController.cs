using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class ArchonBattleshipController : ShipController
    {
        private IBroadcastingAnimation _unfurlAnimation;
        private bool _isUnfurled;

        public BarrelWrapper laser;
        public GameObject bones;

        protected override bool IsOperational => _isUnfurled;
        public override bool IsUltra => true;
        public override Vector2 Size => base.Size * 2;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            Helper.AssertIsNotNull(bones, laser);

            _unfurlAnimation = bones.GetComponent<IBroadcastingAnimation>();
            Assert.IsNotNull(_unfurlAnimation);
            _unfurlAnimation.AnimationDone += _unfurlAnimation_AnimationDone;

            TargetProxy[] colliderTargetProxies = GetComponentsInChildren<TargetProxy>(includeInactive: true);
            foreach (TargetProxy targetProxy in colliderTargetProxies)
            {
                targetProxy.Initialise(this);
            }
        }

        public override void Activate(BuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);
            _isUnfurled = false;
        }

        protected override void OnShipCompleted()
        {
            // Show bones, starting unfurl animation
            bones.SetActive(true);

            // Delay normal setup (movement, turrets) until the unfurl animation has completed
        }

        private void _unfurlAnimation_AnimationDone(object sender, EventArgs e)
        {
            _isUnfurled = true;
            base.OnShipCompleted();
        }

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        public override float OptimalArmamentRangeInM
        {
            get
            {
                return laser.RangeInM;
            }
        }

        protected override ISoundKey EngineSoundKey => SoundKeys.Engines.Archon;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            return new List<IBarrelWrapper>()
            { 
                laser 
            };
        }

        protected override void InitialiseTurrets()
        {
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            laser.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction);
        }

        protected override List<SpriteRenderer> GetMainRenderer()
        {
            // Like turrets, the archon has no main renderer :)
            return new List<SpriteRenderer>();
        }

        protected override void Deactivate()
        {
            base.Deactivate();
            bones.SetActive(false);
        }
    }
}

using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
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
        private IBarrelWrapper _directFireAntiSea, _directFireAntiAir1, _directFireAntiAir2, _missileLauncherFront, _missileLauncherRear;
        private IBroadcastingAnimation _unfurlAnimation;

        public GameObject bones;

        public override bool IsUltra => true;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            Assert.IsNotNull(bones);

            _unfurlAnimation = bones.GetComponent<IBroadcastingAnimation>();
            Assert.IsNotNull(_unfurlAnimation);
            _unfurlAnimation.AnimationDone += _unfurlAnimation_AnimationDone;
        }

        protected override void OnShipCompleted()
        {
            // Show bones, starting unfurl animation
            bones.SetActive(true);

            // Delay normal setup (movement, turrets) until the unfurl animation has completed
        }

        private void _unfurlAnimation_AnimationDone(object sender, EventArgs e)
        {
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
                // FELIX  Fix :P
                return 12;
                //// Rear missile launcher and direct fire anti sea will both also be in range.
                //return _missileLauncherFront.RangeInM;
            }
        }

        protected override ISoundKey EngineSoundKey => SoundKeys.Engines.Archon;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();

            // FELIX  Fix :P
            //_directFireAntiSea = transform.FindNamedComponent<IBarrelWrapper>("GravityAffectedAntiSea");
            //turrets.Add(_directFireAntiSea);

            //// Missile launchers
            //_missileLauncherFront = transform.FindNamedComponent<IBarrelWrapper>("MissileLauncherFront");
            //turrets.Add(_missileLauncherFront);

            //_missileLauncherRear = transform.FindNamedComponent<IBarrelWrapper>("MissileLauncherRear");
            //turrets.Add(_missileLauncherRear);

            //// Anti air
            //_directFireAntiAir1 = transform.FindNamedComponent<IBarrelWrapper>("DirectBurstFireAntiAir1");
            //turrets.Add(_directFireAntiAir1);

            //_directFireAntiAir2 = transform.FindNamedComponent<IBarrelWrapper>("DirectBurstFireAntiAir2");
            //turrets.Add(_directFireAntiAir2);

            return turrets;
        }

        protected override void InitialiseTurrets()
        {
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            // FELIX  Fix :P
            //_directFireAntiSea.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction, SoundKeys.Firing.BigCannon);
            //_missileLauncherFront.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction);
            //_missileLauncherRear.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction);
            //_directFireAntiAir1.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction, SoundKeys.Firing.AntiAir);
            //_directFireAntiAir2.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction, SoundKeys.Firing.AntiAir);
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

using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils.Factories;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class FlakTurtleController : ShipController
    {
        private IBarrelWrapper _flakturret;
        private SectorShieldController _shieldController;
        public float armamentRange;

        public override float OptimalArmamentRangeInM => armamentRange;
        public bool keepDistanceFromEnemyCruiser;
        public override bool KeepDistanceFromEnemyCruiser => keepDistanceFromEnemyCruiser;

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();

            _flakturret = gameObject.GetComponentInChildren<IBarrelWrapper>();
            Assert.IsNotNull(_flakturret);
            turrets.Add(_flakturret);

            return turrets;
        }

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            _shieldController = GetComponentInChildren<SectorShieldController>(includeInactive: true);
            _shieldController.gameObject.SetActive(false);
            base.StaticInitialise(parent, healthBar);
            _shieldController.StaticInitialise();
        }

        protected override void InitialiseTurrets()
        {
            _flakturret.Initialise(this, _cruiserSpecificFactories, SoundKeys.Firing.AntiAir);
        }


        protected override void OnBuildableCompleted()
        {
            _shieldController.Initialise(Faction, FactoryProvider.Sound.SoundPlayer, TargetType.Ships);
            _shieldController.gameObject.SetActive(false);
            base.OnBuildableCompleted();
            _flakturret.ApplyVariantStats(this);
            _shieldController.gameObject.SetActive(true);
        }

        public override void StopMoving()
        {
            base.StopMoving();

            //Shield swings backwards animation 
        }
    }
}

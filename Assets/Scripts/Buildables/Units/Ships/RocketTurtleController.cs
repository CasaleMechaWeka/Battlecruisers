using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class RocketTurtleController : ShipController
    {
        private IBarrelWrapper _missileLauncher;
        private SectorShieldController _shieldController;

        public override float OptimalArmamentRangeInM => _missileLauncher.RangeInM;

        public GameObject BaseSprite;

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();

            _missileLauncher = gameObject.GetComponentInChildren<IBarrelWrapper>();
            Assert.IsNotNull(_missileLauncher);
            turrets.Add(_missileLauncher);

            return turrets;
        }

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings)
        {
            _shieldController = GetComponentInChildren<SectorShieldController>(includeInactive: true);
            _shieldController.gameObject.SetActive(false);
            base.StaticInitialise(parent, healthBar, commonStrings);
            _shieldController.StaticInitialise(commonStrings);
        }

        protected override void InitialiseTurrets()
        {
            _missileLauncher.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.RocketLauncher);
        }


        protected override void OnBuildableCompleted()
        {
            _shieldController.Initialise(Faction, _factoryProvider.Sound.SoundPlayer, TargetType.Ships);
            _shieldController.gameObject.SetActive(false);
            base.OnBuildableCompleted();
            _missileLauncher.ApplyVariantStats(this);
            _shieldController.gameObject.SetActive(true);
            BaseSprite.SetActive(true);
        }

        public override void StopMoving()
        {
            base.StopMoving();

            //Shield swings backwards animation 
        }
    }
}

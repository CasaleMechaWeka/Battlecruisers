using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class DestroyerController : AnimatedShipController
    {
        private IBarrelWrapper _directFireAntiSea, _mortar, _directFireAntiAir, _samSite, _missileLauncher;

        private float _optimalArmamentRangeInM;
        public override float OptimalArmamentRangeInM => _optimalArmamentRangeInM;

        private const float OPTIMAL_RANGE_BUFFER_IN_M = 1;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);
            _optimalArmamentRangeInM = FindOptimalArmamentRangeInM();
        }

        /// <summary>
        /// Want to:
        /// + Stay out of range of mortars
        /// + But go close enough for more than one destroyer to attack
        /// </summary>
        private float FindOptimalArmamentRangeInM()
        {
            // This is the range at which an enemy mortar will be able to attack :)
            return _mortar.RangeInM + Size.x / 2 + OPTIMAL_RANGE_BUFFER_IN_M;
        }

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();

            // Anti ship turret
            _directFireAntiSea = transform.FindNamedComponent<IBarrelWrapper>("DirectFireAntiSea");
            turrets.Add(_directFireAntiSea);

            // Mortar
            _mortar = transform.FindNamedComponent<IBarrelWrapper>("Mortar");
            turrets.Add(_mortar);

            // Anti air turret
            _directFireAntiAir = transform.FindNamedComponent<IBarrelWrapper>("DirectBurstFireAntiAir");
			turrets.Add(_directFireAntiAir);

            // SAM site
            _samSite = transform.FindNamedComponent<IBarrelWrapper>("SamSite");
            turrets.Add(_samSite);

            // Missile launcher
            _missileLauncher = transform.FindNamedComponent<IBarrelWrapper>("MissileLauncher");
            turrets.Add(_missileLauncher);

            return turrets;
		}

        protected override void InitialiseTurrets()
        {
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            _directFireAntiSea.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction, SoundKeys.Firing.BigCannon);
            _mortar.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction, SoundKeys.Firing.BigCannon);
            _missileLauncher.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction);
            _directFireAntiAir.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction, SoundKeys.Firing.AntiAir);
            _samSite.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction);
		}
 
        protected override List<SpriteRenderer> GetNonTurretRenderers()
        {
            List<SpriteRenderer> renderers = base.GetNonTurretRenderers();

            SpriteRenderer wheelRenderer = transform.FindNamedComponent<SpriteRenderer>("WheelAnimation/Wheel");
            renderers.Add(wheelRenderer);

            return renderers;
        }
    }
}

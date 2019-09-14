using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class FrigateController : ShipController
	{
        private IBarrelWrapper _directFireAntiSea, _mortar, _directFireAntiAir;

        private float _optimalArmamentRangeInM;
        public override float OptimalArmamentRangeInM => _optimalArmamentRangeInM;

        protected override ISoundKey EngineSoundKey => SoundKeys.Engines.Frigate;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);
            _optimalArmamentRangeInM = FindOptimalArmamentRangeInM();
        }
		
        /// <summary>
        /// Enemy detector is in ship center, but longest range barrel (mortar) is behind
        /// ship center.  Want to only stop once barrel is in range, so make optimal 
        /// armament range be less than the longest range barrel.
        private float FindOptimalArmamentRangeInM()
        {
            return _mortar.RangeInM - (Mathf.Abs(transform.position.x - _mortar.Position.x));
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

            return turrets;
		}

        protected override void InitialiseTurrets()
        {
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            _directFireAntiSea.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction, SoundKeys.Firing.BigCannon);
            _mortar.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction, SoundKeys.Firing.BigCannon);
            _directFireAntiAir.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction, SoundKeys.Firing.AntiAir);
		}
	}
}

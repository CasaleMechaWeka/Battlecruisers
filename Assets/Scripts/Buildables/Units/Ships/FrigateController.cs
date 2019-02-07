using System.Collections.Generic;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class FrigateController : ShipController
	{
        private IBarrelWrapper _directFireAntiSea, _mortar, _directFireAntiAir;

        private float _optimalArmamentRangeInM;
        public override float OptimalArmamentRangeInM { get { return _optimalArmamentRangeInM; } }

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Units.Frigate; } }
        protected override ISoundKey EngineSoundKey { get { return SoundKeys.Engines.Frigate; } }

        protected override void OnStaticInitialised()
        {
            base.OnStaticInitialised();

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

            IObservableCollection<IBoostProvider> dummyFireRateBoostProviders = _factoryProvider.GlobalBoostProviders.DummyBoostProviders;

            _directFireAntiSea.Initialise(this, _factoryProvider, enemyFaction, dummyFireRateBoostProviders, SoundKeys.Firing.BigCannon);
            _mortar.Initialise(this, _factoryProvider, enemyFaction, dummyFireRateBoostProviders, SoundKeys.Firing.BigCannon);
            _directFireAntiAir.Initialise(this, _factoryProvider, enemyFaction, dummyFireRateBoostProviders, SoundKeys.Firing.AntiAir);
		}
	}
}

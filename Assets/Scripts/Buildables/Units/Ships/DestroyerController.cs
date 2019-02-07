using System.Collections.Generic;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class DestroyerController : ShipController
	{
        private IBarrelWrapper _directFireAntiSea, _mortar, _directFireAntiAir, _samSite, _missileLauncher;

        private float _optimalArmamentRangeInM;
        public override float OptimalArmamentRangeInM { get { return _optimalArmamentRangeInM; } }
		
        protected override ISoundKey EngineSoundKey { get { return SoundKeys.Engines.Destroyer; } }
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Units.Destroyer; } }

        protected override void OnStaticInitialised()
        {
            base.OnStaticInitialised();

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
            return _mortar.RangeInM + Size.x / 2;
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

            _directFireAntiSea.Initialise(this, _factoryProvider, enemyFaction, SoundKeys.Firing.BigCannon);
            _mortar.Initialise(this, _factoryProvider, enemyFaction, SoundKeys.Firing.BigCannon);
            _missileLauncher.Initialise(this, _factoryProvider, enemyFaction);
            _directFireAntiAir.Initialise(this, _factoryProvider, enemyFaction, SoundKeys.Firing.AntiAir);
            _samSite.Initialise(this, _factoryProvider, enemyFaction);
		}
	}
}

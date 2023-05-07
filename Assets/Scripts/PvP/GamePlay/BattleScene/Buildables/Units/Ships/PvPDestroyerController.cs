using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public class PvPDestroyerController : PvPAnimatedShipController
    {
        private IPvPBarrelWrapper _directFireAntiSea, _mortar, _directFireAntiAir, _missileLauncher;//, _samSite;

        private float _optimalArmamentRangeInM;
        public override float OptimalArmamentRangeInM => _optimalArmamentRangeInM;

        private const float OPTIMAL_RANGE_BUFFER_IN_M = 1;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);
            _optimalArmamentRangeInM = FindOptimalArmamentRangeInM();
        }

        /// <summary>
        /// Want to:
        /// + Stay out of range of mortars
        /// + But go close enough for more than one destroyer to attack
        /// </summary>
        private float FindOptimalArmamentRangeInM()
        {
            return _mortar.RangeInM + Size.x / 2 + OPTIMAL_RANGE_BUFFER_IN_M;
        }

        protected override IList<IPvPBarrelWrapper> GetTurrets()
        {
            IList<IPvPBarrelWrapper> turrets = new List<IPvPBarrelWrapper>();

            // Anti ship turret
            _directFireAntiSea = transform.FindNamedComponent<IPvPBarrelWrapper>("DirectFireAntiSea");
            turrets.Add(_directFireAntiSea);

            // Mortar
            _mortar = transform.FindNamedComponent<IPvPBarrelWrapper>("Mortar");
            turrets.Add(_mortar);

            // Anti air turret
            _directFireAntiAir = transform.FindNamedComponent<IPvPBarrelWrapper>("DirectBurstFireAntiAir");
            turrets.Add(_directFireAntiAir);

            // SAM site
            //_samSite = transform.FindNamedComponent<IBarrelWrapper>("SamSite");
            //turrets.Add(_samSite);

            // Missile launcher
            _missileLauncher = transform.FindNamedComponent<IPvPBarrelWrapper>("MissileLauncher");
            turrets.Add(_missileLauncher);

            return turrets;
        }

        protected override void InitialiseTurrets()
        {
            _directFireAntiSea.Initialise(this, _factoryProvider, _cruiserSpecificFactories, PvPSoundKeys.PvPFiring.BigCannon);
            _mortar.Initialise(this, _factoryProvider, _cruiserSpecificFactories, PvPSoundKeys.PvPFiring.BigCannon);
            _missileLauncher.Initialise(this, _factoryProvider, _cruiserSpecificFactories, PvPSoundKeys.PvPFiring.RocketLauncher);
            _directFireAntiAir.Initialise(this, _factoryProvider, _cruiserSpecificFactories, PvPSoundKeys.PvPFiring.AntiAir);
            //_samSite.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.Missile);
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

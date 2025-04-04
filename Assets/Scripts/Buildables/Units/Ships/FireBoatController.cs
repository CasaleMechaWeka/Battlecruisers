using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class FireBoatController : ShipController
    {
        private IBarrelWrapper _directFireFlame;

        private float _optimalArmamentRangeInM;
        public override float OptimalArmamentRangeInM => _optimalArmamentRangeInM;
        public override bool KeepDistanceFromEnemyCruiser => false;

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
            float distanceFromTarget = Mathf.Abs(transform.position.x - _directFireFlame.Position.x);
            float adjustedRange = _directFireFlame.RangeInM - distanceFromTarget;

            // Subtract a constant to make the ship get closer than its max range.
            float overlapDistance = 5.0f;

            return adjustedRange - overlapDistance;
        }

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();

            // Anti ship turret
            _directFireFlame = transform.FindNamedComponent<IBarrelWrapper>("DirectFireFlame");
            turrets.Add(_directFireFlame);

            return turrets;
        }

        protected override void InitialiseTurrets()
        {
            _directFireFlame.Initialise(this, _cruiserSpecificFactories, SoundKeys.Firing.BigCannon);
        }
    }
}
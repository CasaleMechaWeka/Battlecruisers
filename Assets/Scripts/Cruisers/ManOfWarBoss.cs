using BattleCruisers.Utils.Localisation;
using UnityEngine;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Data.Settings;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    public class ManOfWarBoss : Cruiser
    {
        public ArchonBattleshipController unit;
        public GameObject parentWrapper;
        public HealthBarController healthBar;
        bool started = false;
        public SpriteRenderer spriteRenderer;
        public LaserTurretStats laserTurretStats;
        public override void Initialise(ICruiserArgs args)
        {
            isCruiser = false;
            base.Initialise(args);
            unit.Initialise(_uiManager, FactoryProvider);
            unit.Activate(this, _enemyCruiser, CruiserSpecificFactories);
            started = true;
            maxHealth = 0 + unit.maxHealth;
            _healthTracker.SetHealth(0 + unit.maxHealth);
            spriteRenderer.sprite = null;
            Assert.IsNotNull(laserTurretStats);
        }

        public override void FixedUpdate()
        {
            if (started)
            {
                _healthTracker.SetHealth(0 + unit.Health);
                //Debug.Log(unit.Health);
            }
        }

        public override void StaticInitialise(ILocTable commonStrings)
        {
            isCruiser = false;
            base.StaticInitialise(commonStrings);
            unit.StaticInitialise(parentWrapper, healthBar, commonStrings);
        }

        public override void AdjustStatsByDifficulty(Difficulty AIDifficulty)
        {
            switch (AIDifficulty)
            {
                case Difficulty.Normal:
                    laserTurretStats.fireRatePerS *= .625f;
                    laserTurretStats.damagePerS *= .75f;
                    unit.maxHealth *= .2f;
                    break;

                case Difficulty.Hard:
                    laserTurretStats.fireRatePerS *= .8f;
                    laserTurretStats.damagePerS *= 1;
                    unit.maxHealth *= .4f;
                    break;

                case Difficulty.Harder:
                    laserTurretStats.fireRatePerS *= 1f;
                    laserTurretStats.damagePerS *= 1;
                    unit.maxHealth *= 1;
                    break;

                default:
                    unit.maxHealth = 150000;
                    break;
            }
            unit.SetHealthToMax();
            maxHealth = 0 + unit.maxHealth;
            unit.SetHealthToMax();
            _healthTracker.SetHealth(0 + unit.Health);
            unit.buildTimeInS = maxHealth;
            //Debug.Log("Ship blocker: " + shipBlocker.transform.localPosition);
            //Debug.Log(maxHealth);
            //Debug.Log(MaxHealth);
        }
    }

}
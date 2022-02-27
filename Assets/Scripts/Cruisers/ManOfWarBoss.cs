using BattleCruisers.Utils.Localisation;
using UnityEngine;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Settings;

namespace BattleCruisers.Cruisers
{
    public class ManOfWarBoss : Cruiser
    {
        public ArchonBattleshipController unit;
        public GameObject parentWrapper;
        public HealthBarController healthBar;
        bool started = false;
        public SpriteRenderer spriteRenderer;
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
        }

        public override void Update()
        {
            if (started)
            {
                _healthTracker.SetHealth(0 + unit.Health);
                Debug.Log(unit.Health);
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
                    unit.maxHealth = 4000;
                    break;

                case Difficulty.Hard:
                    unit.maxHealth = 6000;
                    break;

                case Difficulty.Harder:
                    unit.maxHealth = 10000;
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
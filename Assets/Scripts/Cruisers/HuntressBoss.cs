using BattleCruisers.Utils.Localisation;
using UnityEngine;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Settings;
using System;

namespace BattleCruisers.Cruisers
{
    public class HuntressBoss : Cruiser
    {
        public HuntressUnit unit;
        public GameObject parentWrapper;
        public HealthBarController healthBar;
        bool started = false;
        public SpriteRenderer spriteRenderer;
        public float timeToFinish;
        public GameObject shipBlocker;
        public override void Initialise(ICruiserArgs args)
        {
            isCruiser = false;
            base.Initialise(args);
            unit.Initialise(_uiManager, FactoryProvider);
            unit.Activate(this, _enemyCruiser, CruiserSpecificFactories);
            started = true;
            maxHealth = unit.maxHealth;
            _healthTracker.SetHealth(unit.Health);
            spriteRenderer.sprite = null;
            unit.RearingStarted += _RearingStarted;
        }

        public override void Update()
        {
            if (started)
            {
                _healthTracker.SetHealth(unit.Health);
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
                    unit.maxHealth = 50000;
                    break;

                case Difficulty.Hard:
                    unit.maxHealth = 70000;
                    break;

                case Difficulty.Harder:
                    unit.maxHealth = 100000;
                    break;

                default:
                    unit.maxHealth = 150000;
                    break;
            }
            unit.SetHealthToMax();
            maxHealth = unit.maxHealth;
            unit.SetHealthToMax();
            _healthTracker.SetHealth(unit.Health);
            //Debug.Log("Ship blocker: " + shipBlocker.transform.localPosition);
        }

        private void _RearingStarted(object sender, EventArgs e)
        {
            shipBlocker.transform.Translate(new Vector3(-9,0,0));
            //Debug.Log("Moved ship blocker: " + shipBlocker.transform.localPosition);
        }
    }

}
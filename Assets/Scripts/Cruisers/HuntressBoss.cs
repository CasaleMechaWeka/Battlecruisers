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
            maxHealth = 0 + unit.maxHealth;
            _healthTracker.SetHealth(0 + unit.maxHealth);
            spriteRenderer.sprite = null;
            unit.RearingStarted += _RearingStarted;
        }

        public override void Update()
        {
            if (started)
            {
                if (_healthTracker.Health > unit.Health)
                {
                    _healthTracker.SetHealth(0 + unit.Health);
                }
                //Debug.Log(maxHealth);
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
                    unit.maxHealth = 8000;
                    break;

                case Difficulty.Hard:
                    unit.maxHealth = 15000;
                    break;

                case Difficulty.Harder:
                    unit.maxHealth = 30000;
                    break;

                default:
                    unit.maxHealth = 75000;
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

        private void _RearingStarted(object sender, EventArgs e)
        {
            shipBlocker.transform.Translate(new Vector3(-6.5f,0,0));
            //Debug.Log("Moved ship blocker: " + shipBlocker.transform.localPosition);
        }
    }

}
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Data.Settings;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPHuntressBoss : PvPCruiser
    {
        public PvPHuntressUnit unit;
        public GameObject parentWrapper;
        public PvPHealthBarController healthBar;
        bool started = false;
        public SpriteRenderer spriteRenderer;
        public float timeToFinish;
        public GameObject shipBlocker;
        //public override Vector2 Size => base.Size*300;
        public override void Initialise(IPvPCruiserArgs args)
        {
            isPvPCruiser = false;
            base.Initialise(args);
            unit.Initialise(/* _uiManager,*/ FactoryProvider);
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
            isPvPCruiser = false;
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
                    unit.maxHealth = 20000;
                    unit.minigunStats.damage *= 1.5f;
                    break;

                case Difficulty.Harder:
                    unit.maxHealth = 50000;
                    unit.minigunStats.damage *= 2.5f;
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
        }

        private void _RearingStarted(object sender, EventArgs e)
        {
            shipBlocker.transform.Translate(new Vector3(-6.5f, 0, 0));
            //Debug.Log("Moved ship blocker: " + shipBlocker.transform.localPosition);
        }
    }

}
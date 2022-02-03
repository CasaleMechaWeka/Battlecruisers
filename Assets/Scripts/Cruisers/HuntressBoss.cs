using BattleCruisers.Utils.Localisation;
using UnityEngine;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Settings;

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
                    unit.maxHealth = 5000;
                    break;

                case Difficulty.Hard:
                    unit.maxHealth = 7000;
                    break;

                case Difficulty.Harder:
                    unit.maxHealth = 10000;
                    break;

                default:
                    unit.maxHealth = 15000;
                    break;
            }
            unit.SetHealthToMax();
            maxHealth = unit.maxHealth;
            _healthTracker.SetHealth(unit.Health);
        }
    }

}
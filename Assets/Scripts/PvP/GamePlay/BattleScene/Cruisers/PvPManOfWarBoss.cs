using BattleCruisers.Utils.Localisation;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Data.Settings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPManOfWarBoss : PvPCruiser
    {
        public PvPArchonBattleshipController unit;
        public GameObject parentWrapper;
        public PvPHealthBarController healthBar;
        bool started = false;
        public SpriteRenderer spriteRenderer;
        public override void Initialise(IPvPCruiserArgs args)
        {
            isPvPCruiser = false;
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
                //Debug.Log(unit.Health);
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
                    unit.maxHealth = 2000;
                    break;

                case Difficulty.Hard:
                    unit.maxHealth = 4000;
                    break;

                case Difficulty.Harder:
                    unit.maxHealth = 8000;
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
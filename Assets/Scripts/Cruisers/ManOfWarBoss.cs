using BattleCruisers.Utils.Localisation;
using UnityEngine;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Buildables;

namespace BattleCruisers.Cruisers
{
    public class ManOfWarBoss : Cruiser
    {
        public ArchonBattleshipController unit;
        public GameObject parentWrapper;
        public HealthBarController healthBar;
        public override void Initialise(ICruiserArgs args)
        {
            isCruiser = false;
            base.Initialise(args);
            unit.Initialise(_uiManager, FactoryProvider);
            unit.Activate(this, _enemyCruiser, CruiserSpecificFactories);
        }

        public override void Update()
        {
            
        }

        public override void StaticInitialise(ILocTable commonStrings)
        {
            isCruiser = false;
            base.StaticInitialise(commonStrings);
            unit.StaticInitialise(parentWrapper, healthBar, commonStrings);
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.PvP.ArenaScreen.Buttons
{  
    public class ArenaBattleButton : ArenaScreenButton
    {
        protected override void OnClicked()
        {
            base.OnClicked();
            _multiplayScreen.GotoArena();
        }
    }
}


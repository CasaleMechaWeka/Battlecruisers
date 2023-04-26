using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click
{
    public interface IPvPClickHandler
    {
        event EventHandler SingleClick;
        event EventHandler DoubleClick;
    }
}



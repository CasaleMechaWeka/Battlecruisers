using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI
{
    public interface IPvPPointerUpDownEmitter
    {
        event EventHandler PointerDown;
        event EventHandler PointerUp;
    }
}

using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.Escape
{
    public interface IPvPEscapeDetector : IManagedDisposable
    {
        event EventHandler EscapePressed;
    }
}
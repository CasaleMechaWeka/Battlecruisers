using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.Escape
{
    public interface IPvPEscapeDetector : IPvPManagedDisposable
    {
        event EventHandler EscapePressed;
    }
}
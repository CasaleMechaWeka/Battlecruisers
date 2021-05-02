using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys.Escape
{
    public interface IEscapeDetector : IManagedDisposable
    {
        event EventHandler EscapePressed;
    }
}
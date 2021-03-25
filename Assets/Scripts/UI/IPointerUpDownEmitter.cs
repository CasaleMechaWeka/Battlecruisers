using System;

namespace BattleCruisers.UI
{
    public interface IPointerUpDownEmitter
    {
        event EventHandler PointerDown;
        event EventHandler PointerUp;
    }
}

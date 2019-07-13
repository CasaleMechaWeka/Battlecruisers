using System;

namespace UnityCommon.UI
{
    public interface IClickableEmitter
    {
        event EventHandler Clicked;
    }
}

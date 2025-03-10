using System;

namespace BattleCruisers.UI.Common.Click
{
    public interface IClickHandler
    {
        event EventHandler SingleClick;
        event EventHandler DoubleClick;
    }
}
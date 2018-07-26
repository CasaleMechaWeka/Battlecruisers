using System;

namespace BattleCruisers.UI.Common
{
    public interface IClickHandler
    {
        event EventHandler SingleClick;
        event EventHandler DoubleClick;

        void OnClick(float timeSinceGameStartInS);
    }
}
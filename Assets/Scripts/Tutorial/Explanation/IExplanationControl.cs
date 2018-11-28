using BattleCruisers.UI;
using System;

namespace BattleCruisers.Tutorial.Explanation
{
    public interface IExplanationControl : ITextDisplayer, IPanel
    {
        event EventHandler DismissButtonClicked;
    }
}
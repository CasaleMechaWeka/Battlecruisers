using System;

namespace BattleCruisers.Tutorial.Explanation
{
    public interface IExplanationControl : ITextDisplayer
    {
        event EventHandler DismissButtonClicked;
    }
}
using System;

namespace BattleCruisers.Tutorial
{
    public interface ITextDisplayer
    {
        event EventHandler TextChanged;

        string Text { get; }

        void DisplayText(string textToDisplay);
    }
}

using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial
{
    public interface IPvPTextDisplayer
    {
        event EventHandler TextChanged;

        string Text { get; }

        void DisplayText(string textToDisplay);
    }
}

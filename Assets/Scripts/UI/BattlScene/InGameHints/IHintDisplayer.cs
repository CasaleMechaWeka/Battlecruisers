using System;

namespace BattleCruisers.UI.BattleScene.InGameHints
{
    public interface IHintDisplayer
    {
        // FELIX  Only show hint of have not shown this hint before :)
        void ShowHint(string hint);
    }
}
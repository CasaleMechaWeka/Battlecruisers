using System;

namespace BattleCruisers.UI.BattleScene.InGameHints
{
    public interface IFactoryMonitor
    {
        event EventHandler FactoryCompleted;
        event EventHandler UnitChosen;
    }
}
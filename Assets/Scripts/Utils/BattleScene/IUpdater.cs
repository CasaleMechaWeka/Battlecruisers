using System;

namespace BattleCruisers.Utils.BattleScene
{
    public interface IUpdater
    {
        event EventHandler Updated;
    }
}
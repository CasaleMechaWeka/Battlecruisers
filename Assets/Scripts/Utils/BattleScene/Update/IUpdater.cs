using System;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public interface IUpdater
    {
        event EventHandler Updated;
    }
}
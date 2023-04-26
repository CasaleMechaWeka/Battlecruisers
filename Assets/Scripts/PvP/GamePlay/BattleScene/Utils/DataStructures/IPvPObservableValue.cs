using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures
{
    public interface IPvPObservableValue<T>
    {
        T Value { get; }

        event EventHandler ValueChanged;
    }
}
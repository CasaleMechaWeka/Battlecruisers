using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties
{
    public interface IPvPBroadcastingProperty<T>
    {
        T Value { get; }
        event EventHandler ValueChanged;
    }
}



using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties
{
    public interface IPvPSettableBroadcastingProperty<T>
    {
        T Value { get; set; }

        event EventHandler ValueChanged;
    }
}
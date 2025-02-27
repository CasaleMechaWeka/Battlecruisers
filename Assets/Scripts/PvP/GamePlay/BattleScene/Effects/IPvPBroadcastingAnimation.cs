using BattleCruisers.Effects;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects
{
    public interface IPvPBroadcastingAnimation : IAnimation
    {
        event EventHandler AnimationDone;
        event EventHandler AnimationStarted;
    }
}
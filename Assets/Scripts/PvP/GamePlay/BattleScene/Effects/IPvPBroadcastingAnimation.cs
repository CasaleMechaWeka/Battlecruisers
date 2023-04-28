using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects
{
    public interface IPvPBroadcastingAnimation : IPvPAnimation
    {
        event EventHandler AnimationDone;
        event EventHandler AnimationStarted;
    }
}
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects
{
    public class PvPBroadcastingAnimationController : MonoBehaviour, IPvPBroadcastingAnimation
    {
        public event EventHandler AnimationDone;
        public event EventHandler AnimationStarted;

        public void Done()
        {
            AnimationDone?.Invoke(this, EventArgs.Empty);
        }

        public void Started()
        {
            //Debug.Log("Holy shit it worked!");
            AnimationStarted?.Invoke(this, EventArgs.Empty);
        }

        public void Play()
        {
            // Plays automatically :/
        }
    }
}
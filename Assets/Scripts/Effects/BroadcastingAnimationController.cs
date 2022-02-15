using System;
using UnityEngine;

namespace BattleCruisers.Effects
{
    public class BroadcastingAnimationController : MonoBehaviour, IBroadcastingAnimation
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
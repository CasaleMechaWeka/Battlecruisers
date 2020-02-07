using System;
using UnityEngine;

namespace BattleCruisers.Effects
{
    public class BroadcastingAnimationController : MonoBehaviour, IBroadcastingAnimation
    {
        public event EventHandler AnimationDone;

        public void Done()
        {
            AnimationDone?.Invoke(this, EventArgs.Empty);
        }

        public void Play()
        {
            // Plays automatically :/
        }
    }
}
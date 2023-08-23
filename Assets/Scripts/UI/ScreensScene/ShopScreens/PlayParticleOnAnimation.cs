using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene
{
    public class PlayParticleOnAnimation : MonoBehaviour
    {
        public ParticleSystem[] particleSystems;
        public void PlayParticle()
        {
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Play();
            }
        }
    }
}

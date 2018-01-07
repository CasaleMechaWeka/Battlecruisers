using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Sounds
{
    public class SoundsTestGod : MonoBehaviour
    {
        //public AudioClip audioClip;

        void Start()
        {
            string mp3File = "AA-machine-gun";
            string wavFile = "archon-engine-seamless-loop";

            //AudioClip mp3Audio = GetAudio(mp3File);
            //AudioSource.PlayClipAtPoint(mp3Audio, default(Vector3));

            AudioClip wavAudio = GetAudio(wavFile);
            AudioSource.PlayClipAtPoint(wavAudio, default(Vector3));

            //AudioSource.PlayClipAtPoint(audioClip, default(Vector3));
        }

        private AudioClip GetAudio(string audioName)
        {
            AudioClip audioClip = Resources.Load<AudioClip>("Sounds/" + audioName);

            if (audioClip == null)
            {
                throw new ArgumentException("Invalid audio name: " + audioName);
            }

            return audioClip;
        }
    }
}

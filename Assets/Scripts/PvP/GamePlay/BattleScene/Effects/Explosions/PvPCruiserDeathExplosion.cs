using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions
{
    public class PvPCruiserDeathExplosion : PvPExplosionController
    {
        private AudioSourceGroup _audioSources;
        public PvPBodykitWreck[] wrecks;
        public SpriteRenderer[] wreckRenders;

        public IPoolable<Vector3> Initialise(ISettingsManager settingsManager)
        {
            IPoolable<Vector3> explosion = base.Initialise();

            Assert.IsNotNull(settingsManager);

            AudioSource[] platformAudioSources = GetComponentsInChildren<AudioSource>(includeInactive: true);
            Assert.IsTrue(platformAudioSources.Length != 0);
            IList<IAudioSource> audioSources
                = platformAudioSources
                    .Select(audioSource => (IAudioSource)new AudioSourceBC(audioSource))
                    .ToList();

            _audioSources = new AudioSourceGroup(settingsManager, audioSources);

            return explosion;
        }

        public void ApplyBodykitWreck(int id_bodykit)
        {
            if (id_bodykit < 0)
                return;
            BODYKIT_TYPE bodykit_type = (BODYKIT_TYPE)id_bodykit;
            foreach (PvPBodykitWreck wreck in wrecks)
            {
                if (wreck.type == bodykit_type)
                {
                    for (int i = 0; i < wreckRenders.Length; i++)
                    {
                        wreckRenders[i].sprite = wreck.sprites[i];
                    }
                }
            }
        }
    }

    public enum BODYKIT_TYPE
    {
        BODYKIT000, BODYKIT001, BODYKIT002, BODYKIT003, BODYKIT004,
        BODYKIT005, BODYKIT006, BODYKIT007, BODYKIT008, BODYKIT009,
        BODYKIT010, BODYKIT011, BODYKIT012, BODYKIT013, BODYKIT014,
        BODYKIT015, BODYKIT016, BODYKIT017, BODYKIT018, BODYKIT019,
        BODYKIT020, BODYKIT021, BODYKIT022, BODYKIT023, BODYKIT024,
        BODYKIT025, BODYKIT026, BODYKIT027, BODYKIT028, BODYKIT029,
        BODYKIT030, BODYKIT031, BODYKIT032, BODYKIT033, BODYKIT034,
        BODYKIT035, BODYKIT036, BODYKIT037, BODYKIT038, BODYKIT039,
        BODYKIT040, BODYKIT041, BODYKIT042, BODYKIT043, BODYKIT044,
        BODYKIT045, BODYKIT046, BODYKIT047, BODYKIT048, BODYKIT049,
        BODYKIT050, BODYKIT051, BODYKIT052
    }

    [Serializable]
    public class PvPBodykitWreck
    {
        public BODYKIT_TYPE type;
        public Sprite[] sprites;
    }
}

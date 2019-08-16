using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.BattleScene
{
    public class BattleSceneGodComponents : MonoBehaviour, IBattleSceneGodComponents
    {
        public IDeferrer Deferrer { get; private set; }
        public IDeferrer TimeScaleDeferrer { get; private set; }
        public IAudioSource AudioSource { get; private set; }
        public CloudInitialiser CloudInitialiser { get; private set; }
        public SkyboxInitialiser SkyboxInitialiser { get; private set; }
        public ICamera Camera { get; private set; }

        private UpdaterProvider _updaterProvider;
        public IUpdaterProvider UpdaterProvider => _updaterProvider;

        public void Initialise()
        {
            Deferrer = new Deferrer();

            TimeScaleDeferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(TimeScaleDeferrer);

            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            AudioSource = new AudioSourceBC(platformAudioSource);

            CloudInitialiser = GetComponent<CloudInitialiser>();
            Assert.IsNotNull(CloudInitialiser);

            SkyboxInitialiser = GetComponent<SkyboxInitialiser>();
            Assert.IsNotNull(SkyboxInitialiser);

            Camera platformCamera = FindObjectOfType<Camera>();
            Assert.IsNotNull(platformCamera);
            Camera = new CameraBC(platformCamera);

            _updaterProvider = GetComponentInChildren<UpdaterProvider>();
            Assert.IsNotNull(_updaterProvider);
            _updaterProvider.Initialise();
        }
    }
}
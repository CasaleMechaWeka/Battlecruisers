using BattleCruisers.Projectiles.Trackers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.Cameras;
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
        public IVariableDelayDeferrer VariableDelayDeferrer { get; private set; }
        public IHighlightFactory HighlightFactory { get; private set; }
        public IAudioSource AudioSource { get; private set; }
        public CloudInitialiser CloudInitialiser { get; private set; }
        public SkyboxInitialiser SkyboxInitialiser { get; private set; }
        public ICamera Camera { get; private set; }
        public IMarkerFactory MarkerFactory { get; private set; }

        public void Initialise()
        {
            Deferrer = GetComponent<IDeferrer>();
            Assert.IsNotNull(Deferrer);

            VariableDelayDeferrer = GetComponent<IVariableDelayDeferrer>();
            Assert.IsNotNull(VariableDelayDeferrer);

            HighlightFactory = GetComponent<IHighlightFactory>();
            Assert.IsNotNull(HighlightFactory);

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

            MarkerFactory = GetComponent<MarkerFactory>();
            Assert.IsNotNull(MarkerFactory);
        }
    }
}
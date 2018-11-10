using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes
{
    public class BattleSceneGodComponents : MonoBehaviour, IBattleSceneGodComponents
    {
        public IDeferrer Deferrer { get; private set; }
        public IVariableDelayDeferrer VariableDelayDeferrer { get; private set; }
        public IHighlightFactory HighlightFactory { get; private set; }
        public IAudioSource AudioSource { get; private set; }

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
        }
    }
}
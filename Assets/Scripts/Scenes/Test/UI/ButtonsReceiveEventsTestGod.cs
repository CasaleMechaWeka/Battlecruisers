using BattleCruisers.UI;
using BattleCruisers.UI.Sound;
using NSubstitute;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.UI
{
    public class ButtonsReceiveEventsTestGod : MonoBehaviour
    {
        public ActionButton actionButton;

        void Start()
        {
            Assert.IsNotNull(actionButton);

            ISingleSoundPlayer soundPlayer = Substitute.For<ISingleSoundPlayer>();
            actionButton.Initialise(soundPlayer, SweetAction);
        }

        private void SweetAction()
        {
            Debug.Log("SweetAction");
        }
    }
}
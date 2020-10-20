using BattleCruisers.UI;
using BattleCruisers.UI.Sound;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.UI
{
    public class ButtonsReceiveEventsTestGod : MonoBehaviour
    {
        void Start()
        {
            ISingleSoundPlayer soundPlayer = Substitute.For<ISingleSoundPlayer>();

            ActionButton[] buttons = FindObjectsOfType<ActionButton>();

            foreach (ActionButton button in buttons)
            {
                button.Initialise(soundPlayer, SweetAction);
            }
        }

        private void SweetAction()
        {
            Debug.Log("SweetAction");
        }
    }
}
using UnityEngine;
using System.Collections;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.UI.ScreensScene.ChooseDifficultyScreen
{
    public class DifficultyButtonController : ElementWithClickSound
    {
        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IDismissableEmitter parent)
        {
            base.Initialise(soundPlayer, parent: parent);

            // FELIX :D
        }
    }
}
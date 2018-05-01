using System;

namespace BattleCruisers.Tutorial
{
    public interface ITutorialManager
    {
        event EventHandler TutorialCompleted;

        void StartTutorial();
    }
}

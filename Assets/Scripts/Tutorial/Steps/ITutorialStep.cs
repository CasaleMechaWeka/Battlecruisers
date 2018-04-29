using System;

namespace BattleCruisers.Tutorial.Steps
{
    public interface ITutorialStep
    {
        void Start(Action completionCallback);
    }
}

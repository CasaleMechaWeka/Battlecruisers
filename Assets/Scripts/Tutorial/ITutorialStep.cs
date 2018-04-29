using System;

namespace BattleCruisers.Tutorial
{
    public interface ITutorialStep
    {
        void Start(Action completionCallback);
    }
}

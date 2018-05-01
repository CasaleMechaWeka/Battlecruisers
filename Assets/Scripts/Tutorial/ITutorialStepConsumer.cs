using System;

namespace BattleCruisers.Tutorial
{
    public interface ITutorialStepConsumer
    {
        event EventHandler Completed;

        void StartConsuming();
    }
}

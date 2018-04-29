using System.Collections.Generic;
using BattleCruisers.Tutorial.Steps;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class TutorialStepConsumer
    {
        private readonly Queue<ITutorialStep> _tutorialSteps;

        public TutorialStepConsumer(Queue<ITutorialStep> tutorialSteps)
        {
            Assert.IsNotNull(tutorialSteps);
            _tutorialSteps = tutorialSteps;

            StartNextTask();
        }

        private void StartNextTask()
        {
            if (_tutorialSteps.Count != 0)
            {
                _tutorialSteps.Dequeue().Start(StartNextTask);
            }
        }
    }
}

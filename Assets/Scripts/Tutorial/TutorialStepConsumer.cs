using System;
using System.Collections.Generic;
using BattleCruisers.Tutorial.Steps;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class TutorialStepConsumer : ITutorialStepConsumer
    {
        private readonly Queue<ITutorialStep> _tutorialSteps;
		
		public event EventHandler Completed;

        public TutorialStepConsumer(Queue<ITutorialStep> tutorialSteps)
        {
            Assert.IsNotNull(tutorialSteps);
            _tutorialSteps = tutorialSteps;
        }

        public void StartConsuming()
        {
			StartNextTask();
        }

        private void StartNextTask()
        {
            if (_tutorialSteps.Count != 0)
            {
                _tutorialSteps.Dequeue().Start(StartNextTask);
            }
            else Completed?.Invoke(this, EventArgs.Empty);
        }
    }
}

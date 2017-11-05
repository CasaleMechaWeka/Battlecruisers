using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors
{
    public abstract class TargetProcessorWrapper : MonoBehaviour, ITargetProcessorWrapper
    {
        private ITargetConsumer _targetConsumer;
        private ITargetProcessor _targetProcessor;

        private bool IsInitialised { get { return _targetProcessor != null; } }

        public void Initialise(
            ITargetsFactory targetsFactory,
            ITargetConsumer targetConsumer,
            Faction enemyFaction,
            IList<TargetType> attackCapabilities,
            float detectionRangeInM,
            float minRangeInM = 0)
        {
            Helper.AssertIsNotNull(targetsFactory, targetConsumer, attackCapabilities);

            _targetConsumer = targetConsumer;

            _targetProcessor = CreateTargetProcessor(targetsFactory, enemyFaction, attackCapabilities, detectionRangeInM, minRangeInM);
            _targetProcessor.AddTargetConsumer(_targetConsumer);
        }

        protected abstract ITargetProcessor CreateTargetProcessor(
            ITargetsFactory targetsFactory,
            Faction enemyFaction,
            IList<TargetType> attackCapabilities,
            float detectionRangeInM,
            float minRangeInM);

        public void StartProvidingTargets()
        {
            _targetProcessor.StartProcessingTargets();
        }
		
        public void Dispose()
        {
            if (IsInitialised)
            {
                CleanUp();
            }
        }

        protected virtual void CleanUp()
        {
			_targetProcessor.RemoveTargetConsumer(_targetConsumer);
            _targetProcessor.Dispose();
			_targetProcessor = null;
        }
    }
}

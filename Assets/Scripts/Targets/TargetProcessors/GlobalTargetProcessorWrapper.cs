using System.Collections.Generic;
using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class GlobalTargetProcessorWrapper : MonoBehaviour, ITargetProcessorWrapper
    {
        private ITargetConsumer _targetConsumer;
        private ITargetProcessor _targetProcessor;

        private bool IsInitialised { get { return _targetProcessor != null; } }

        public void StartProvidingTargets(ITargetsFactory targetsFactory, ITargetConsumer targetConsumer, 
            Faction enemyFaction, float detectionRangeInM, IList<TargetType> attackCapabilities)
        {
            _targetConsumer = targetConsumer;
			_targetProcessor = targetsFactory.OffensiveBuildableTargetProcessor;
            _targetProcessor.AddTargetConsumer(_targetConsumer);
        }

        public void Dispose()
        {
            if (IsInitialised)
            {
                _targetProcessor.RemoveTargetConsumer(_targetConsumer);
                _targetProcessor = null;
            }
        }
    }
}

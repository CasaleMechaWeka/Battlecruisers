using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Targets.TargetProcessors.Ranking.Wrappers;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public abstract class TargetProcessorWrapper : MonoBehaviour, ITargetProcessorWrapper
    {
        private ITargetConsumer _targetConsumer;
        private ITargetProcessor _targetProcessor;
        private bool _isProvidingTargets;

        private bool IsInitialised { get { return _targetProcessor != null; } }

        public void Initialise(
            ITargetsFactory targetsFactory,
            ITargetConsumer targetConsumer,
            Faction enemyFaction,
            IList<TargetType> attackCapabilities,
            float maxRangeInM,
            float minRangeInM = 0)
        {
            Helper.AssertIsNotNull(targetsFactory, targetConsumer, attackCapabilities);

            _targetConsumer = targetConsumer;

            ITargetRanker targetRanker = CreateTargetRanker(targetsFactory);

            _targetProcessor = CreateTargetProcessor(targetsFactory, targetRanker, enemyFaction, attackCapabilities, maxRangeInM, minRangeInM);

            _isProvidingTargets = false;
        }

        protected virtual ITargetRanker CreateTargetRanker(ITargetsFactory targetsFactory)
        {
			ITargetRankerWrapper targetRankerWrapper = GetComponent<ITargetRankerWrapper>();
			Assert.IsNotNull(targetRankerWrapper);
			return targetRankerWrapper.CreateTargetRanker(targetsFactory);
        }

        protected abstract ITargetProcessor CreateTargetProcessor(
            ITargetsFactory targetsFactory,
            ITargetRanker targetRanker,
            Faction enemyFaction,
            IList<TargetType> attackCapabilities,
            float maxRangeInM,
            float minRangeInM);

        public void StartProvidingTargets()
        {
            Assert.IsFalse(_isProvidingTargets);
            _isProvidingTargets = true;

			_targetProcessor.AddTargetConsumer(_targetConsumer);
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

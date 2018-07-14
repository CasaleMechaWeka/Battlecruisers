using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Targets.TargetProcessors.Ranking.Wrappers;
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

        public void Initialise(ITargetProcessorArgs args)
        {
            Assert.IsNotNull(args);

            _targetConsumer = args.TargetConsumer;
			_isProvidingTargets = false;

            ITargetRanker targetRanker = CreateTargetRanker(args.TargetsFactory);
            _targetProcessor = CreateTargetProcessor(args, targetRanker);
        }

        protected virtual ITargetRanker CreateTargetRanker(ITargetsFactory targetsFactory)
        {
			ITargetRankerWrapper targetRankerWrapper = GetComponent<ITargetRankerWrapper>();
			Assert.IsNotNull(targetRankerWrapper);
			return targetRankerWrapper.CreateTargetRanker(targetsFactory);
        }

        protected abstract ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args, ITargetRanker targetRanker);

        public void StartProvidingTargets()
        {
            Assert.IsFalse(_isProvidingTargets);
            _isProvidingTargets = true;

			_targetProcessor.AddTargetConsumer(_targetConsumer);
            _targetProcessor.StartProcessingTargets();
        }
		
        public void DisposeManagedState()
        {
            if (IsInitialised)
            {
                CleanUp();
            }
        }

        protected virtual void CleanUp()
        {
			_targetProcessor.RemoveTargetConsumer(_targetConsumer);
            _targetProcessor.DisposeManagedState();
			_targetProcessor = null;
        }
    }
}

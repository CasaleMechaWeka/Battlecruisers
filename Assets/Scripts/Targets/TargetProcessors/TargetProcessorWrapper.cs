using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    // FELIX  Rename to Initialiser/Factory?
    public abstract class TargetProcessorWrapper : MonoBehaviour, ITargetProcessorWrapper
    {
        private ITargetConsumer _targetConsumer;
        private ITargetProcessor _targetProcessor;
        private bool _isProvidingTargets;

        // FELIX  Remove this?  Should never be disposed if Initalise() was not first called :/
        private bool IsInitialised { get { return _targetProcessor != null; } }

        public ITargetProcessor Initialise(ITargetProcessorArgs args)
        {
            Assert.IsNotNull(args);

            _targetConsumer = args.TargetConsumer;
			_isProvidingTargets = false;

            _targetProcessor = CreateTargetProcessor(args);
            return _targetProcessor;
        }

        protected abstract ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args);

        // FELIX  Remove this method?  Hm...  Caller's responsibility to:  1. Add consumer  2. Start processor
        public void StartProvidingTargets()
        {
            Assert.IsFalse(_isProvidingTargets);
            _isProvidingTargets = true;

            if (_targetConsumer != null)
            {
    			_targetProcessor.AddTargetConsumer(_targetConsumer);
            }
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

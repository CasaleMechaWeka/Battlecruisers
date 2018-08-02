using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public abstract class TargetProcessorWrapper : MonoBehaviour, ITargetProcessorWrapper
    {
        private ITargetConsumer _targetConsumer;
        private ITargetProcessor _targetProcessor;
        private bool _isProvidingTargets;

        // FELIX  Remove this?  Should never be disposed if Initalise() was not first called :/
        private bool IsInitialised { get { return _targetProcessor != null; } }

        public void Initialise(ITargetProcessorArgs args)
        {
            Assert.IsNotNull(args);

            _targetConsumer = args.TargetConsumer;
			_isProvidingTargets = false;

            _targetProcessor = CreateTargetProcessor(args);
        }

        protected abstract ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args);

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

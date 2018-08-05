using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    // FELIX  Rename to Initialiser/Factory?
    public abstract class TargetProcessorWrapper : MonoBehaviour, ITargetProcessorWrapper
    {
        private ITargetConsumer _targetConsumer;
        private ITargetProcessor _targetProcessor;

        // FELIX  Remove this?  Should never be disposed if Initalise() was not first called :/
        private bool IsInitialised { get { return _targetProcessor != null; } }

        // FELIX  Simplify args, don't need target consumer anymore :)
        public ITargetProcessor Initialise(ITargetProcessorArgs args)
        {
            Assert.IsNotNull(args);

            _targetConsumer = args.TargetConsumer;

            _targetProcessor = CreateTargetProcessor(args);
            return _targetProcessor;
        }

        protected abstract ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args);

        public void DisposeManagedState()
        {
            if (IsInitialised)
            {
                CleanUp();
            }
        }

        // FELIX  Should not be this wrapper's resonsibility :/  Wrapper should simply act as a factory.
        protected virtual void CleanUp()
        {
			_targetProcessor.RemoveTargetConsumer(_targetConsumer);
            _targetProcessor.DisposeManagedState();
			_targetProcessor = null;
        }
    }
}

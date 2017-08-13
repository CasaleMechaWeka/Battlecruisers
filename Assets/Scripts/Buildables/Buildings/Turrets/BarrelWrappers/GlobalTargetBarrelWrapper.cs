using BattleCruisers.Targets.TargetProcessors;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class GlobalTargetBarrelWrapper : BarrelWrapper
    {
		private ITargetProcessor _targetProcessor;

		protected override ITargetProcessor GetTargetProcessor()
        {
            _targetProcessor = _factoryProvider.TargetsFactory.OffensiveBuildableTargetProcessor;
            return _targetProcessor;
		}
		
        public override void Dispose()
		{
			_targetProcessor.RemoveTargetConsumer(this);
			_targetProcessor = null;	
		}
    }
}

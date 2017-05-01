using BattleCruisers.Buildables;

namespace BattleCruisers.Targets
{
	public interface ITargetConsumer
	{
		IFactionable Target { set; }
	}
}

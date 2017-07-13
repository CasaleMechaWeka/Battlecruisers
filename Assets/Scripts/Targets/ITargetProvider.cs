using BattleCruisers.Buildables;

namespace BattleCruisers.Targets
{
	public interface ITargetProvider
	{
		ITarget Target { get; }
	}
}

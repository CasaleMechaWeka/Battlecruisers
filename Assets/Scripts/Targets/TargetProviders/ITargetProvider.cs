using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetProviders
{
	public interface ITargetProvider
	{
		ITarget Target { get; }
	}
}

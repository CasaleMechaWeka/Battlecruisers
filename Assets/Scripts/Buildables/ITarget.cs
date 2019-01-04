using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using UnityEngine;

namespace BattleCruisers.Buildables
{
    public enum Faction
	{
		Blues, Reds
	}

	public enum TargetType
	{
		Aircraft, Ships, Cruiser, Buildings, Rocket, Satellite
	}

	/// <summary>
	/// Used for prioritising targets, so do NOT change order!
	/// </summary>
	public enum TargetValue
	{
		Low, Medium, High
	}

    public interface ITarget : IDamagable, IRepairable, IHighlightable, IMaskHighlightable
    {
		Faction Faction { get; }
		TargetType TargetType { get; }
		Vector2 Velocity { get; }
		ReadOnlyCollection<TargetType> AttackCapabilities { get; }
		TargetValue TargetValue { get; }

        Vector2 Position { get; set; }
        Quaternion Rotation { get; set; }
	}
}

using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Tutorial.Highlighting.Masked;
using System.Collections.ObjectModel;
using UnityCommon.PlatformAbstractions;
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

    public interface ITarget : IDamagable, IRepairable, IMaskHighlightable
    {
		Faction Faction { get; }
		TargetType TargetType { get; }
		Vector2 Velocity { get; }
		ReadOnlyCollection<TargetType> AttackCapabilities { get; }
		TargetValue TargetValue { get; }
        Color Color { set; }
        bool IsInScene { get; }
        Vector2 Size { get; }
        ITransform Transform { get; }

        Vector2 Position { get; set; }
        Quaternion Rotation { get; set; }
	}
}

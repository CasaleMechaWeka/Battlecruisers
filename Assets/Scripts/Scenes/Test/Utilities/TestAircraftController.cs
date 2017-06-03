using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Aircraft;
using System;

namespace BattleCruisers.Scenes.Test.Utilities
{
	public class TestAircraftController : AircraftController
	{
		private TargetType _targetType;

		public override TargetType TargetType { get { return _targetType; } }

		public void SetTargetType(TargetType targetType)
		{
			_targetType = targetType;
		}
	}
}


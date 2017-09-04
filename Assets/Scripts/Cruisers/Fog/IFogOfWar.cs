using System;

namespace BattleCruisers.Cruisers.Fog
{
	public interface IFogOfWar
	{
		bool IsFogEnabled { get; }

		event EventHandler IsFogEnabledChanged;

        void UpdateIsEnabled(int numOfFriendlyStealthGenerators, int numOfEnemySpySatellites);
	}
}

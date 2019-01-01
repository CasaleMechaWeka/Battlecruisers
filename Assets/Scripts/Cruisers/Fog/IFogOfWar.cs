using System;

namespace BattleCruisers.Cruisers.Fog
{
    public enum FogStrength
    {
        Weak, Strong
    }

	public interface IFogOfWar
	{
		bool IsFogEnabled { get; }

		event EventHandler IsFogEnabledChanged;

        void UpdateIsEnabled(int numOfFriendlyStealthGenerators, int numOfEnemySpySatellites);
	}
}

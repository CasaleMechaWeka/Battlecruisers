using System;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Drones;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using UnityEngine;

namespace BattleCruisers.Buildables
{
    public enum BuildableState
	{
		NotStarted, InProgress, Paused, Completed
	}

	public class BuildProgressEventArgs : EventArgs
	{
		public IBuildable Buildable { get; private set; }

		public BuildProgressEventArgs(IBuildable buildable)
		{
			Buildable = buildable;
		}
	}

    public interface IBuildable : ITarget, IComparableItem, IBoostable
    {
        /// <summary>
        /// 0-1
        /// </summary>
        float BuildProgress { get; }
        BuildableState BuildableState { get; }
        float Damage { get; }
        Vector3 Size { get; }
        IDroneConsumer DroneConsumer { get; }
        SlotType SlotType { get; }
        new Vector2 Position { get; set; }
        Quaternion Rotation { set; }
        int NumOfDronesRequired { get; }
        float BuildTimeInS { get; }

		event EventHandler StartedConstruction;
		event EventHandler CompletedBuildable;
		event EventHandler<BuildProgressEventArgs> BuildableProgress;

        void StaticInitialise();
		void StartConstruction();
		void InitiateDelete();
	}
}

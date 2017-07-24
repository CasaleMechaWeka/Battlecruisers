using System;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Utils;
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

    // FELIX Use this instead of concrete type whereever possible?
    public interface IBuildable : ITarget
    {
        /// <summary>
        /// 0-1
        /// </summary>
        float BuildProgress { get; }
        BuildableState BuildableState { get; }
        float Damage { get; }
        Vector3 Size { get; }
        Sprite Sprite { get; }
        IDroneConsumer DroneConsumer { get; }
        SlotType SlotType { get; }

		event EventHandler StartedConstruction;
		event EventHandler CompletedBuildable;
		event EventHandler<BuildProgressEventArgs> BuildableProgress;

		void Initialise(ICruiser parentCruiser, ICruiser enemyCruiser, UIManager uiManager, IFactoryProvider factoryProvider);
		void StartConstruction();
		void InitiateDelete();
	}
}

using System;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
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

    public interface IBuildable : ITarget, IComparableItem, IClickable
    {
        /// <summary>
        /// 0-1
        /// </summary>
        float BuildProgress { get; }
        BuildableState BuildableState { get; }
		int NumOfDronesRequired { get; }
		float BuildTimeInS { get; }
		IDroneConsumer DroneConsumer { get; }
        ICommand ToggleDroneConsumerFocusCommand { get; }
        float CostInDroneS { get; }
        ReadOnlyCollection<IDamageCapability> DamageCapabilities { get; }
		IBoostable BuildProgressBoostable { get; }
        bool IsInitialised { get; }
        ICruiser ParentCruiser { get; }
  
        new Vector2 Position { get; set; }
        Quaternion Rotation { get; set; }

		event EventHandler StartedConstruction;
		event EventHandler CompletedBuildable;
		event EventHandler<BuildProgressEventArgs> BuildableProgress;
        event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;

        void StaticInitialise();
		void StartConstruction();
        void InitiateDelete();
        void CancelDelete();
	}
}

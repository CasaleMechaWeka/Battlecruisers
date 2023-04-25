using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public enum PvPBuildableState
    {
        NotStarted, InProgress, Paused, Completed
    }

    public class PvPBuildProgressEventArgs : EventArgs
    {
        public IPvPBuildable Buildable { get; }

        public PvPBuildProgressEventArgs(IPvPBuildable buildable)
        {
            Buildable = buildable;
        }
    }

    public interface IPvPBuildable : IPvPTarget, IPvPComparableItem, IPvPClickableEmitter
    {
        /// <summary>
        /// 0-1
        /// </summary>
        float BuildProgress { get; }
        PvPBuildableState BuildableState { get; }
        int NumOfDronesRequired { get; }
        float BuildTimeInS { get; }
        IPvPDroneConsumer DroneConsumer { get; }
        IPvPCommand ToggleDroneConsumerFocusCommand { get; }
        float CostInDroneS { get; }
        ReadOnlyCollection<IPvPDamageCapability> DamageCapabilities { get; }
        IPvPBoostable BuildProgressBoostable { get; }
        bool IsInitialised { get; }
        IPvPCruiser ParentCruiser { get; }
        IPvPCruiser EnemyCruiser { get; }
        IPvPHealthBar HealthBar { get; }
        string PrefabName { get; }
        string keyName { get; set; }
        event EventHandler StartedConstruction;
        event EventHandler CompletedBuildable;
        event EventHandler<PvPBuildProgressEventArgs> BuildableProgress;
        event EventHandler<PvPDroneNumChangedEventArgs> DroneNumChanged;

        void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings);
        void Initialise(IPvPUIManager uiManager, IPvPFactoryProvider factoryProvider);
        void StartConstruction();
    }
}
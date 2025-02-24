using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories
{
    public interface IPvPFactory : IPvPBuilding
    {
        UnitCategory UnitCategory { get; }
        int NumOfDrones { get; }
        IPvPBuildableWrapper<IPvPUnit> UnitWrapper { get; set; }
        IPvPUnit UnitUnderConstruction { get; set; }
        IObservableValue<bool> IsUnitPaused { get; }
        LayerMask UnitLayerMask { get; }
        IAudioClipWrapper SelectedSound { get; }
        IAudioClipWrapper UnitSelectedSound { get; }

        void StartBuildingUnit(IPvPBuildableWrapper<IPvPUnit> unit, int variantIndex);
        void StartBuildingUnit(IPvPBuildableWrapper<IPvPUnit> unit);
        void StopBuildingUnit();
        void PauseBuildingUnit();
        void ResumeBuildingUnit();
        // void OnNewFactoryChosen();

        event EventHandler<PvPUnitStartedEventArgs> UnitStarted;
        event EventHandler<PvPUnitCompletedEventArgs> UnitCompleted;
        event EventHandler NewUnitChosen;
        event EventHandler UnitUnderConstructionDestroyed;
        // event EventHandler<PvPUnitStartedEventArgs> NewFactoryChosen;
    }
}

using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using System;
using System.Diagnostics;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public class PvPUnitBuildProgressTrigger : IPvPUnitBuildProgressTrigger
    {
        private readonly IPvPUnitBuildProgress _unitBuildProgress;

        private IPvPFactory _factory;

        public PvPUnitBuildProgressTrigger(IPvPUnitBuildProgress unitBuildProgress)
        {
            Assert.IsNotNull(unitBuildProgress);
            _unitBuildProgress = unitBuildProgress;
        }

        public IPvPFactory Factory
        {
            private get => _factory;
            set
            {
                if (_factory != null)
                {
                    //     _factory.NewFactoryChosen -= _factory_NewChosen;
                    _factory.UnitStarted -= _factory_UnitStarted;
                    _factory.NewUnitChosen -= _factory_NewUnitChosen;
                    _factory.UnitUnderConstructionDestroyed -= _factory_UnitUnderConstructionDestroyed;
                }

                _factory = value;

                if (_factory != null)
                {
                    ShowBuildProgressIfNecessary(_factory.UnitUnderConstruction);
                    //    ShowBuildProgressIfNecessary(_factory.UnitWrapper?.Buildable);
                    //    _factory.NewFactoryChosen += _factory_NewChosen;
                    _factory.UnitStarted += _factory_UnitStarted;
                    _factory.NewUnitChosen += _factory_NewUnitChosen;
                    _factory.UnitUnderConstructionDestroyed += _factory_UnitUnderConstructionDestroyed;
                }
            }
        }

        private void _factory_UnitStarted(object sender, PvPUnitStartedEventArgs e)
        {
            UnityEngine.Debug.Log(" =====> UnitStarted!!!");
            ShowBuildProgressIfNecessary(e.StartedUnit);
        }

        private void _factory_NewUnitChosen(object sender, EventArgs e)
        {
            UnityEngine.Debug.Log(" =====> NewUnitChosen!!!");
            ShowBuildProgressIfNecessary(_factory.UnitWrapper?.Buildable);
        }

        private void _factory_UnitUnderConstructionDestroyed(object sender, EventArgs e)
        {
            UnityEngine.Debug.Log(" =====> UnitUnderConstructionDestroyed!!!");
            ShowBuildProgressIfNecessary(_factory.UnitWrapper?.Buildable);
        }

        private void ShowBuildProgressIfNecessary(IPvPUnit unit)
        {
            _unitBuildProgress.ShowBuildProgressIfNecessary(unit, _factory);
        }
    }
}
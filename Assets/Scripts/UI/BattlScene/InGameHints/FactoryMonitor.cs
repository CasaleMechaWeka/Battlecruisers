using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.InGameHints
{
    // FELIX  Use, test
    public class FactoryMonitor : IFactoryMonitor
    {
        private readonly ICruiserBuildingMonitor _buildingMonitor;

        public event EventHandler FactoryCompleted;
        public event EventHandler UnitChosen;

        public FactoryMonitor(ICruiserBuildingMonitor buildingMonitor)
        {
            Assert.IsNotNull(buildingMonitor);

            _buildingMonitor = buildingMonitor;
            _buildingMonitor.BuildingCompleted += _buildingMonitor_BuildingCompleted;
        }

        private void _buildingMonitor_BuildingCompleted(object sender, BuildingCompletedEventArgs e)
        {
            if (e.CompletedBuilding is IFactory factory)
            {
                FactoryCompleted?.Invoke(this, EventArgs.Empty);

                factory.NewUnitChosen += Factory_NewUnitChosen;
                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void Factory_NewUnitChosen(object sender, EventArgs e)
        {
            UnitChosen?.Invoke(this, EventArgs.Empty);
        }

        private void Factory_Destroyed(object sender, DestroyedEventArgs e)
        {
            IFactory factory = e.DestroyedTarget.Parse<IFactory>();
            factory.NewUnitChosen -= Factory_NewUnitChosen;
            factory.Destroyed -= Factory_Destroyed;
        }
    }
}
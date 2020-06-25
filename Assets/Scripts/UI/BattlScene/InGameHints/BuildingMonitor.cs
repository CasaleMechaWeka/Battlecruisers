using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.InGameHints
{
    // FELIX  Use, test :)
    public class BuildingMonitor : IBuildingMonitor
    {
        private readonly ICruiserController _aiCruiser;

        public event EventHandler AirFactoryStarted;
        public event EventHandler NavalFactoryStarted;
        public event EventHandler OffensiveStarted;

        public BuildingMonitor(ICruiserController aiCruiser)
        {
            Assert.IsNotNull(aiCruiser);

            _aiCruiser = aiCruiser;
            _aiCruiser.BuildingStarted += _aiCruiser_BuildingStarted;
        }

        private void _aiCruiser_BuildingStarted(object sender, BuildingStartedEventArgs e)
        {
            if (e.StartedBuilding is AirFactory)
            {
                AirFactoryStarted?.Invoke(this, EventArgs.Empty);
            }
            else if (e.StartedBuilding is NavalFactory)
            {
                NavalFactoryStarted?.Invoke(this, EventArgs.Empty);
            }
            else if(e.StartedBuilding.Category == BuildingCategory.Offence
                || e.StartedBuilding.Category == BuildingCategory.Ultra)
            {
                OffensiveStarted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
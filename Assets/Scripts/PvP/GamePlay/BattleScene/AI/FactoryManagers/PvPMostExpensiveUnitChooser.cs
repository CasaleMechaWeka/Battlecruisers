using System.Collections.Generic;
using System.Linq;
using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
{
    /// <summary>
	/// Chooses the most expensive acceptable unit.
	/// 
	/// Updates the chosen unit every time the cruiser's number of drones changes.
	/// </summary>
	public class PvPMostExpensiveUnitChooser : PvPUnitChooser
    {
        private readonly IList<IPvPBuildableWrapper<IPvPUnit>> _units;
        private readonly IDroneManager _droneManager;
        private readonly IUnitFilter _unitFilter;

        public PvPMostExpensiveUnitChooser(IList<IPvPBuildableWrapper<IPvPUnit>> units, IDroneManager droneManager, IUnitFilter unitFilter)
        {
            PvPHelper.AssertIsNotNull(units, droneManager, unitFilter);
            Assert.IsTrue(units.Count != 0);

            _units = units;
            _droneManager = droneManager;
            _unitFilter = unitFilter;

            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;

            ChooseUnit();
        }

        private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            ChooseUnit();
        }

        private void ChooseUnit()
        {
            ChosenUnit =
                _units
                    .Where(wrapper => _unitFilter.IsBuildableAcceptable(wrapper.Buildable.NumOfDronesRequired, _droneManager.NumOfDrones))
                    .OrderByDescending(wrapper => wrapper.Buildable.NumOfDronesRequired)
                    .FirstOrDefault();
        }

        public override void DisposeManagedState()
        {
            _droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
        }
    }
}

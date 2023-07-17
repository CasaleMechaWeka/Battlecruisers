using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
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
        private readonly IPvPDroneManager _droneManager;
        private readonly IPvPUnitFilter _unitFilter;

        public PvPMostExpensiveUnitChooser(IList<IPvPBuildableWrapper<IPvPUnit>> units, IPvPDroneManager droneManager, IPvPUnitFilter unitFilter)
        {
            PvPHelper.AssertIsNotNull(units, droneManager, unitFilter);
            Assert.IsTrue(units.Count != 0);

            _units = units;
            _droneManager = droneManager;
            _unitFilter = unitFilter;

            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;

            ChooseUnit();
        }

        private void _droneManager_DroneNumChanged(object sender, PvPDroneNumChangedEventArgs e)
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

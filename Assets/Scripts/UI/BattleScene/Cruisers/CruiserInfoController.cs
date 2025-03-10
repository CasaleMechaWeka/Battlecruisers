using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.ProgressBars;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Cruisers
{
    public class CruiserInfoController : MonoBehaviour, ICruiserInfo
    {
        private HealthBarController _cruiserHealthBar;

		private NumOfDronesController _numOfDrones;
        public IButton NumOfDronesButton => _numOfDrones;

        public void Initialise(ICruiser cruiser)
        {
            Assert.IsNotNull(cruiser);

            _cruiserHealthBar = GetComponentInChildren<HealthBarController>();
            Assert.IsNotNull(_cruiserHealthBar);
            _cruiserHealthBar.Initialise(cruiser);

            _numOfDrones = GetComponentInChildren<NumOfDronesController>();
            Assert.IsNotNull(_numOfDrones);
            _numOfDrones.Initialise(cruiser.DroneManager);
        }
    }
}

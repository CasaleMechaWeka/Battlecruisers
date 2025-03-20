using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class KillCountController : MonoBehaviour, IKillCountController
    {
        private int _targetCostInDroneS;
        private TextMesh _killCountText, _costText;

        private const string KILL_COUNT_PREFIX = "Kill count: ";
        private const string KILL_COST_PREFIX = "Kill cost: ";

        private int _killCount;
        public int KillCount
        {
            get { return _killCount; }
            set
            {
                _killCount = value;

                _killCountText.text = KILL_COUNT_PREFIX + _killCount;

                int costOfKilledTargets = _killCount * _targetCostInDroneS;
                _costText.text = KILL_COST_PREFIX + costOfKilledTargets;
            }
        }

        public void Initialise(int targetCostInDroneS)
        {
            _targetCostInDroneS = targetCostInDroneS;

            TextMesh killCountText = transform.FindNamedComponent<TextMesh>("KillCountText");
            _killCountText = killCountText;

            TextMesh costText = transform.FindNamedComponent<TextMesh>("CostText");
            _costText = costText;

            KillCount = 0;
        }
    }
}

using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class BuildMenuCanvasController : MonoBehaviour
    {
        public BuildableDetailsController BuildableDetails { get; private set; }
        public InBattleCruiserDetailsController CruiserDetails { get; private set; }
        public HealthBarController PlayerCruiserHealthBar { get; private set; }
        public HealthBarController AiCruiserHealthBar { get; private set; }

        public void Initialise()
        {
            BuildableDetails = GetComponentInChildren<BuildableDetailsController>(includeInactive: true);
            Assert.IsNotNull(BuildableDetails);

            CruiserDetails = GetComponentInChildren<InBattleCruiserDetailsController>(includeInactive: true);
            Assert.IsNotNull(CruiserDetails);

            PlayerCruiserHealthBar = GetComponentInChildren<HealthBarController>(includeInactive: true);
            Assert.IsNotNull(PlayerCruiserHealthBar);

            AiCruiserHealthBar = GetComponentInChildren<HealthBarController>(includeInactive: true);
            Assert.IsNotNull(AiCruiserHealthBar);
        }
    }
}

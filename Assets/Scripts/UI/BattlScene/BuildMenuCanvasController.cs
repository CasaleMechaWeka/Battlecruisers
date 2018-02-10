using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class BuildMenuCanvasController : MonoBehaviour, IBuildMenuCanvasController
    {
        public BuildingDetailsController BuildindgDetails { get; private set; }
        public UnitDetailsController UnitDetails { get; private set; }
        public InBattleCruiserDetailsController CruiserDetails { get; private set; }
        public HealthBarController PlayerCruiserHealthBar { get; private set; }
        public HealthBarController AiCruiserHealthBar { get; private set; }

        public void Initialise()
        {
            BuildindgDetails = GetComponentInChildren<BuildingDetailsController>(includeInactive: true);
            Assert.IsNotNull(BuildindgDetails);

            UnitDetails = GetComponentInChildren<UnitDetailsController>(includeInactive: true);
            Assert.IsNotNull(UnitDetails);

            CruiserDetails = GetComponentInChildren<InBattleCruiserDetailsController>(includeInactive: true);
            Assert.IsNotNull(CruiserDetails);

            PlayerCruiserHealthBar = transform.FindNamedComponent<HealthBarController>("PlayerCruiserHealthBar");
            AiCruiserHealthBar = transform.FindNamedComponent<HealthBarController>("AiCruiserHealthBar");
        }
    }
}

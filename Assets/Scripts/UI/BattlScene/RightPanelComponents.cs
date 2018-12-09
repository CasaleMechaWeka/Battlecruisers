using BattleCruisers.UI.Common.BuildableDetails;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class RightPanelComponents
    {
        // FELIX  Rename property to InformatorPanel
        public IInformatorPanel Informator { get; private set; }

        public RightPanelComponents(IInformatorPanel informator)
        {
            Assert.IsNotNull(informator);
            Informator = informator;
        }
    }
}
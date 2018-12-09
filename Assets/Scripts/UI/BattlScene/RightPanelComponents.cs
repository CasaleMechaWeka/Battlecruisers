using BattleCruisers.UI.Common.BuildableDetails;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class RightPanelComponents
    {
        public IInformatorPanel Informator { get; private set; }

        public RightPanelComponents(IInformatorPanel informator)
        {
            Assert.IsNotNull(informator);
            Informator = informator;
        }
    }
}
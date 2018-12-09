using BattleCruisers.UI.Common.BuildableDetails;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class RightPanelComponents
    {
        public IInformatorPanel InformatorPanel { get; private set; }

        public RightPanelComponents(IInformatorPanel informatorPanel)
        {
            Assert.IsNotNull(informatorPanel);
            InformatorPanel = informatorPanel;
        }
    }
}
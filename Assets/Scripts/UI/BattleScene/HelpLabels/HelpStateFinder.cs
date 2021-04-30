using UnityEngine;
using System.Collections;
using BattleCruisers.UI.Panels;
using BattleCruisers.Utils.BattleScene;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    // FELIX  Test :D
    public class HelpStateFinder : IHelpStateFinder
    {
        private readonly ISlidingPanel _informatorPanel, _informatorExtendedPanel, _selectorPanel;
        private readonly IHelpState _bothCollapsed, _selectorShown, _informatorShown, _bothShown;

        public IHelpState FindHelpStat()
        {
            // FELIX
            return null;
        }
    }
}
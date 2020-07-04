using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.InGameHints
{
    public class NonRepeatingHintDisplayer : IHintDisplayer
    {
        private readonly HashSet<string> _hints;
        private readonly IHintDisplayer _coreDisplayer;

        public NonRepeatingHintDisplayer(IHintDisplayer coreDisplayer)
        {
            Assert.IsNotNull(coreDisplayer);
            _coreDisplayer = coreDisplayer;

            _hints = new HashSet<string>();
        }

        public void ShowHint(string hint)
        {
            Assert.IsNotNull(hint);

            if (_hints.Contains(hint))
            {
                return;
            }

            _hints.Add(hint);
            _coreDisplayer.ShowHint(hint);
        }

        public void HideHint(string hint)
        {
            _coreDisplayer.HideHint(hint);
        }
    }
}
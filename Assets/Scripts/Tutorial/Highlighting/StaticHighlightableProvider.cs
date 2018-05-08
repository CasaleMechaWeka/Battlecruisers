using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class StaticHighlightableProvider : IHighlightablesProvider
    {
        private readonly IList<IHighlightable> _highlightables;

        public StaticHighlightableProvider(params IHighlightable[] highlightables)
        {
            Assert.IsNotNull(highlightables);
            Assert.IsTrue(highlightables.Length > 0);

            _highlightables = highlightables;
        }

        public IList<IHighlightable> FindHighlightables()
        {
            return _highlightables;
        }
    }
}

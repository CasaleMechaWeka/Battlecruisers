using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Highlighting
{
    public interface IHighlightablesProvider
    {
        IList<IHighlightable> FindHighlightables();
    }
}

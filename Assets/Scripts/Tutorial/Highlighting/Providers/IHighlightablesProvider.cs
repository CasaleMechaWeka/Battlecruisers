using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Highlighting.Providers
{
    public interface IHighlightablesProvider
    {
        IList<IHighlightable> FindHighlightables();
    }
}

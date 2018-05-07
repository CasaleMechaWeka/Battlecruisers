using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Highlighting.Providers
{
    // FELIX  Delete .Providers namespace :)
    public interface IHighlightablesProvider
    {
        IList<IHighlightable> FindHighlightables();
    }
}

using System.Collections;

namespace BattleCruisers.UI.Loading
{
    public interface ILoadingScreen
    {
        IEnumerator PerformLongOperation(IEnumerator longOperation, string loadingScreenHint = null);
    }
}

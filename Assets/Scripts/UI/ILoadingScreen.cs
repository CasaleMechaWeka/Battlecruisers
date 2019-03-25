using System.Collections;

namespace BattleCruisers.UI
{
    public interface ILoadingScreen
    {
        IEnumerator PerformLongOperation(IEnumerator longOperation, string loadingScreenHint = null);
    }
}

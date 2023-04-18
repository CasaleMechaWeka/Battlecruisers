#if UNITY_EDITOR
using ParrelSync;
#endif
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.Shared.EditorApplication
{
    public class EditorBootstrapper : MonoBehaviour
    {
        public Bootstrapper m_Bootstrapper;
        // Start is called before the first frame update
        void Start()
        {
#if UNITY_EDITOR
            if (ClonesManager.IsClone())
            {
                var argument = ClonesManager.GetArgument();
                if (argument == "server")
                {
                    m_Bootstrapper.OnParellSyncStarted(true);
                }
                else if (argument == "client")
                {
                    m_Bootstrapper.OnParellSyncStarted(false);
                }
            }
            else
            {
                m_Bootstrapper.OnParellSyncStarted(false);
            }
#endif
        }


    }
}


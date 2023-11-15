using UnityEngine;
#if UNITY_EDITOR
using System;
using ParrelSync;
#endif

namespace BattleCruisers.Network.Multiplay.Matchplay.Shared
{
    public class LocalProfileTool
    {
        static string s_LocalProfileSuffix;
        public static string LocalProfileSuffix => s_LocalProfileSuffix ??= GetCloneName();

        static string GetCloneName()
        {
#if UNITY_EDITOR
            if (ClonesManager.IsClone())
            {
                var cloneName = ClonesManager.GetCurrentProject().name;
                return cloneName;
            }
#endif

            return "";
        }
    }
}



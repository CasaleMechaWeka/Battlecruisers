using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Multiplayer.Samples.Utilities;
using VContainer;


namespace BattleCruisers.Network.Multiplay.MultiplayBattleScene.Server
{
    [RequireComponent(typeof(NetcodeHooks))]
    public class MultiplayBattleSceneServerGod : MonoBehaviour
    {
        [SerializeField]
        NetcodeHooks m_NetcodeHooks;
    }
}

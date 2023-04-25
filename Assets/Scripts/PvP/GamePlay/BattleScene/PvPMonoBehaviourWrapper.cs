using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine;
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class PvPMonoBehaviourWrapper : PvPPrefab, IPvPGameObject
    {
        public Vector3 Position
        {
            get { return gameObject.transform.position; }
            set { gameObject.transform.position = value; }
        }

        public bool IsVisible
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); }
        }
    }
}

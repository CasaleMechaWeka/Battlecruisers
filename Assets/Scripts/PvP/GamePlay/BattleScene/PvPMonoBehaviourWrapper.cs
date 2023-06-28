using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine;
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class PvPMonoBehaviourWrapper : PvPPrefab, IPvPGameObject
    {
        public Vector3 Position
        {
            get { return gameObject.transform.position; }
            set
            {
                gameObject.transform.position = value;
                if (IsServer)
                    CallRpc_SetPosition(value);
            }
        }
        private bool _isVisible = false;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                //   gameObject.SetActive(value);
                Transform[] trans = transform.GetComponentsInChildren<Transform>(includeInactive: true);
                foreach (Transform t in trans)
                {
                    if (t != transform)
                        t.gameObject.SetActive(value);
                }
                _isVisible = value;
                if (IsServer)
                    CallRpc_SetVisible(value);
            }
        }

        protected virtual void CallRpc_SetPosition(Vector3 position)
        {

        }

        protected virtual void CallRpc_SetVisible(bool isVisible)
        {

        }
    }
}

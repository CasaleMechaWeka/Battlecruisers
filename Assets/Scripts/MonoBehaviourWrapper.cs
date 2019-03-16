using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers
{
    public class MonoBehaviourWrapper : MonoBehaviour, IGameObject
    {
        public Vector2 Position => gameObject.transform.position;

        public bool IsVisible
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); }
        }
    }
}

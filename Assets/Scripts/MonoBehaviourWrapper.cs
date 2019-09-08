using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers
{
    public class MonoBehaviourWrapper : MonoBehaviour, IGameObject
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

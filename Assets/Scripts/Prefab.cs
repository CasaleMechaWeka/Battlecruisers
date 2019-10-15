using UnityEngine;

namespace BattleCruisers
{
    public class Prefab : MonoBehaviour, IPrefab
    {
        public virtual void StaticInitialise() { }
    }
}
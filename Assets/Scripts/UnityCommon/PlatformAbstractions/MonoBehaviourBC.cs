using UnityEngine;

namespace UnityCommon.PlatformAbstractions
{
    public class MonoBehaviourBC : MonoBehaviour, IGameObject
    {
        public GameObject PlatformObject => gameObject;
        public ITransform Transform { get; private set; }

        public virtual void Initialise()
        {
            Transform = new TransformBC(transform);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

namespace BattleCruisers.UI
{
    public class Spinner : MonoBehaviour
    {
        public float speed = 5f;

        void Update()
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - Time.deltaTime * speed);
        }
    }
}


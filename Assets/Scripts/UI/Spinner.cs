using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.UI
{
    public class Spinner : MonoBehaviour
    {
        public float speed = 5f;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - Time.deltaTime * speed);
        }
    }
}


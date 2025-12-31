using UnityEngine;

namespace BattleCruisers.Utils.Debugging
{
    public class RaycastFromCameraHelper : CheaterBase
    {
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.C))
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPoint.z = Camera.main.transform.position.z;
                Ray ray = new Ray(worldPoint, new Vector3(0, 0, 1));
                RaycastHit2D hitInfo = Physics2D.GetRayIntersection(ray);

                Debug.Log("Hit info collider: " + hitInfo.collider);
            }
        }
    }
}